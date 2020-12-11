using System.ComponentModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using roguelike.Components;
using roguelike.Events;
using SadConsole;

namespace roguelike.Actors
{
    public class Player : Actor
    {
        public Player()
        {
            Components.Add(new EntityComponent());
            Components.Add(new HealthComponent(100));
            Components.Add(new MovementComponent());
            Components.Add(new MeleeAttackComponent(10));

            var entity = Get<EntityComponent>();

            entity.Entity = new SadConsole.Entities.Entity(Color.White, Color.Transparent, '@');
        }

        public bool ProcessKeyboard(SadConsole.Input.Keyboard info)
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
                handled = true;
            }

            if (movement != Point.Zero) {
                var currentPosition = Get<EntityComponent>().Position;

                EventBus.Publish(new BeforeMovementEvent {
                    Actor = this,
                    From = currentPosition,
                    To = currentPosition + movement
                });

                handled = true;
            }

            return handled;
        }
    }
}