using System.Linq;
using System.ComponentModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using roguelike.Actors.Monsters;
using roguelike.Components;
using roguelike.Events;
using roguelike.Utils;
using roguelike.Engine;
using SadConsole;

namespace roguelike.Actors
{
    public class Player : Actor
    {
        public Player()
        {
            Components.Add(new EntityComponent(Color.White, Color.Transparent, '@'));
            Components.Add(new HealthComponent(10));
            Components.Add(new MovementComponent());
            Components.Add(new MeleeAttackComponent(1, 6, 0, 0, 1));
            Components.Add(new StatsComponent {
                Strength = 15,
                Dexterity = 14,
                Constitution = 13,
                Intelligence = 12,
                Wisdom = 10,
                Charisma = 8,
                HitDice = 10,
                ArmorClass = 10
            });
            Components.Add(new ExperienceComponent());
        }

        public bool ProcessKeyboard(SadConsole.Input.Keyboard info, World world)
        {
            var movement = Point.Zero;
            var handled = false;

            if (info.IsKeyPressed(Keys.Up) || info.IsKeyPressed(Keys.NumPad8)) {
                movement = Directions.North;
            } else if (info.IsKeyPressed(Keys.Down) || info.IsKeyPressed(Keys.NumPad2)) {
                movement = Directions.South;
            }

            if (info.IsKeyPressed(Keys.Right) || info.IsKeyPressed(Keys.NumPad6)) {
                movement =  Directions.East;
            } else if (info.IsKeyPressed(Keys.Left) || info.IsKeyPressed(Keys.NumPad4)) {
                movement = Directions.West;
            }

            if (info.IsKeyPressed(Keys.NumPad7)) {
                movement = Directions.NorthWest;
            } else if (info.IsKeyPressed(Keys.NumPad9)) {
                movement = Directions.NorthEast;
            }

            if (info.IsKeyPressed(Keys.NumPad1)) {
                movement = Directions.SouthWest;
            } else if (info.IsKeyPressed(Keys.NumPad3)) {
                movement = Directions.SouthEast;
            }

            if (info.IsKeyPressed(Keys.NumPad5)) {
                world.EventBus.Publish(new InterruptEvent {
                    ActivateIn = 1
                });

                handled = true;
            }

            if (info.IsKeyPressed(Keys.Space)) {
                var stairs = world.CurrentLevel.GetActors<Stairs>().First().Get<EntityComponent>();

                if (stairs.Position == Get<EntityComponent>().Position) {
                    world.DestroyLevel();
                    world.CreateLevel();
                }   
            }

            if (movement != Point.Zero) {
                var currentPosition = Get<EntityComponent>().Position;
                var entity = Get<EntityComponent>();

                var monsters = world.CurrentLevel.GetActors<Monster>();
                Monster attacking = null;
                EntityComponent attackingEntity = null;

                foreach (var monster in monsters) {
                    attackingEntity = monster.Get<EntityComponent>();

                    if (attackingEntity.Position == currentPosition + movement) {
                        attacking = monster;

                        break;
                    }
                }

                if (attacking != null) {
                    var attack = Get<MeleeAttackComponent>();

                    world.EventBus.Publish(new BeforeMeleeAttackEvent {
                        Attacker = this,
                        IntendedTarget = attacking,
                        TargetPoint = attackingEntity.Position,
                        Damage = attack.Damage,
                        ActivateIn = attack.Speed,
                        InterruptOnCancel = true
                    });
                } else {
                    world.EventBus.Publish(new BeforeMovementEvent {
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