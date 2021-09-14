using System.Linq;
using System.ComponentModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using roguelike.Actors.Monsters;
using roguelike.Components;
using roguelike.Events;
using roguelike.Engine;
using SadConsole;
using roguelike.Actors.Items;
using roguelike.Actors.Features;

namespace roguelike.Actors
{
    public class Player : Actor
    {
        public Player()
        {
            Components.Add(new EntityComponent(Color.White, Color.Transparent, '@'));
            Components.Add(new MovementComponent());
            var stats = new StatsComponent
            {
                Strength = 15,
                Dexterity = 14,
                Constitution = 13,
                Intelligence = 12,
                Wisdom = 10,
                Charisma = 8,
                HitDice = 10,
                ArmorClass = 10
            };
            Components.Add(stats);
            Components.Add(new MeleeAttackComponent(1, 1, stats.StrengthModifier, 0, 1));
            Components.Add(new HealthComponent(stats.HitDice + stats.ConstitutionModifier));
            Components.Add(new ExperienceComponent());
            Components.Add(new InventoryComponent());
        }

        public bool ProcessKeyboard(SadConsole.Input.Keyboard info, World world)
        {
            var movement = Point.Zero;
            var handled = false;

            if (info.IsKeyPressed(Keys.Up) || info.IsKeyPressed(Keys.NumPad8))
            {
                movement = Directions.North;
            }
            else if (info.IsKeyPressed(Keys.Down) || info.IsKeyPressed(Keys.NumPad2))
            {
                movement = Directions.South;
            }

            if (info.IsKeyPressed(Keys.Right) || info.IsKeyPressed(Keys.NumPad6))
            {
                movement = Directions.East;
            }
            else if (info.IsKeyPressed(Keys.Left) || info.IsKeyPressed(Keys.NumPad4))
            {
                movement = Directions.West;
            }

            if (info.IsKeyPressed(Keys.NumPad7))
            {
                movement = Directions.NorthWest;
            }
            else if (info.IsKeyPressed(Keys.NumPad9))
            {
                movement = Directions.NorthEast;
            }

            if (info.IsKeyPressed(Keys.NumPad1))
            {
                movement = Directions.SouthWest;
            }
            else if (info.IsKeyPressed(Keys.NumPad3))
            {
                movement = Directions.SouthEast;
            }

            if (info.IsKeyPressed(Keys.NumPad5))
            {
                world.EventBus.Publish(new InterruptEvent
                {
                    ActivateIn = 1
                });

                return true;
            }

            if (info.IsKeyPressed(Keys.I))
            {
                world.InventoryConsole.Update();
                SadConsole.Global.CurrentScreen = world.InventoryConsole;
                SadConsole.Global.CurrentScreen.IsFocused = true;

                return false;
            }

            if (info.IsKeyPressed(Keys.E))
            {
                world.EventConsole.Update();
                SadConsole.Global.CurrentScreen = world.EventConsole;
                SadConsole.Global.CurrentScreen.IsFocused = true;

                return false;
            }

            if (info.IsKeyPressed(Keys.Space))
            {
                var playerE = Get<EntityComponent>();
                var stairs = world.CurrentLevel.GetActors<Stairs>().FirstOrDefault(x => x.Get<EntityComponent>().Position == playerE.Position)?.Get<EntityComponent>();

                if (stairs != null)
                {
                    world.DestroyLevel();
                    world.CreateLevel();
                    world.EventBus.Publish(new InterruptEvent());

                    return true;
                }

                var items = world.CurrentLevel.GetActors<Item>();
                foreach (var item in items)
                {
                    var itemE = item.Get<EntityComponent>();

                    if (itemE == null) continue;

                    if (itemE.Position == playerE.Position)
                    {
                        world.EventBus.Publish(new OnItemPickupEvent
                        {
                            Target = this,
                            Item = item
                        });

                        return true;
                    }
                }
            }

            if (movement != Point.Zero)
            {
                var currentPosition = Get<EntityComponent>().Position;
                var entity = Get<EntityComponent>();

                var monsters = world.CurrentLevel.GetActors<Monster>();
                Monster attacking = null;
                EntityComponent attackingEntity = null;

                foreach (var monster in monsters)
                {
                    attackingEntity = monster.Get<EntityComponent>();

                    if (attackingEntity.Position == currentPosition + movement)
                    {
                        attacking = monster;

                        break;
                    }
                }

                if (attacking != null)
                {
                    var attack = Get<MeleeAttackComponent>();

                    world.EventBus.Publish(new BeforeMeleeAttackEvent
                    {
                        Attacker = this,
                        IntendedTarget = attacking,
                        TargetPoint = attackingEntity.Position,
                        ActivateIn = attack.Speed,
                        InterruptOnCancel = true
                    });
                }
                else
                {
                    world.EventBus.Publish(new BeforeMovementEvent
                    {
                        Actor = this,
                        From = currentPosition,
                        To = currentPosition + movement,
                        ActivateIn = Get<MovementComponent>().Speed,
                        InterruptOnCancel = true
                    });
                }

                handled = true;
            }

            return handled;
        }
    }
}