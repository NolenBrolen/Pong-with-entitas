using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Entitas;
using Entitas.CodeGeneration.Attributes;
using System;

[Game]
public class AllComponents : IComponent { }

[Game, Unique]
public class Player1Component : IComponent { }

[Game, Unique]
public class Player2Component : IComponent { }

[Game, Unique]
public class BallComponent : IComponent { }

[Game]
public class VerticalWallComponent : IComponent { }

[Game]
public class HorizontalWallComponent : IComponent { }

[Game]
public class ViewComponent : IComponent
{
    public GameObject gameObject;
}

[Game]
public class PositionComponent : IComponent
{
    public float x;
    public float y;
}

[Game]
public class AssetComponent : IComponent
{
    public string myPrefabName;
}

[Game]
public class MoveComponent : IComponent { }

[Input]
public class verticalAxis : IComponent
{
    public float value;
}

[Game,Input,CustomPrefix("flag")]
public class DestroyComponent : IComponent { }

public class SpeedComponent : IComponent
{
    public float value;
}

[Game]
public class MovingDirection : IComponent
{
    public bool down;
}

[Input]
public class ChangeColor : IComponent { }

[Game, Unique]
public class ScoreLeft : IComponent { }

[Game, Unique]
public class ScoreRight : IComponent { }

[Game]
public class Velocity : IComponent
{
    public Vector2 value;
}

[Game]
public class Wall : IComponent { }

[Game]
public class CollisionComponent : IComponent
{
    public GameObject owner;
    public GameObject partnerInCrime;
}

[Game]
public class TriggerComponent : IComponent
{
    public GameObject triggerOwner;
    public GameObject triggerObject;
}

[Game, Unique]
public class UiRoot : IComponent
{
    public RectTransform root;
}

[Game]
public class TextComponent : IComponent { }


[Game, Unique]
public class LeftWall : IComponent { }

[Game, Unique]
public class RightWall : IComponent { }


[Game]
public class TriggerWallHit : IComponent {
    public bool leftWall;
}


[Game, Unique]
public class Player1Score : IComponent
{
    public int points;
}

[Game, Unique]
public class Player2Score : IComponent
{
    public int points;
}

[Game, Unique]
public class DelayComponent : IComponent { }


[Game, Unique]
public class ResetPositionComponent : IComponent { }


[Game, Unique]
public class DelayNewRoundComponent : IComponent { }

[Game, Unique]
public class TimeUntilNewRoundComponent : IComponent
{
    public float value;
}

[Game, Unique]
public class TimeUntilResetPositionsComponent : IComponent
{
    public float value;
}

[Game,Unique]
public class DelaySettingsComponent : IComponent {
    public float newRoundDelay;
    public float resetPositionDelay;
}

[Game, Unique]
public class InitialBallVelocityComponent : IComponent
{
    public Vector2 value;
}

[Game, Unique]
public class StartNewRoundComponent : IComponent { }

[Game, Unique]
public class Player1WinnerComponent : IComponent { }

[Game, Unique]
public class Player2WinnerComponent : IComponent { }

[Input, Unique]
public class DisableInputComponent : IComponent { }


[Game, Unique]
public class NewGameButtonComponent : IComponent { }


[Game]
public class UiElementComponent : IComponent { }