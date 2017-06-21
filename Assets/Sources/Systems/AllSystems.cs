using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Entitas;
using Entitas.VisualDebugging.Unity;

public class AllSystems
{
    public static Systems Get(Contexts contexts)
    {
        var systems = new Feature("Game Systems")
        //Initialize
        .Add(new CreateInitialGameStateSystem(contexts))
        .Add(new SpawnPrefab(contexts))
        .Add(new InstantiateUiSystem(contexts))
        .Add(new SetWallsPosition(contexts))
        .Add(new InitializeScoreSystem(contexts))
        .Add(new MovingBall(contexts))

        //Game
        .Add(new RenderPosition(contexts))
        .Add(new GetInput(contexts))
        .Add(new usePlayer1Input(contexts))
        .Add(new ChangeColorSystem(contexts))
        .Add(new DetectTriggerSystem(contexts))

        #region The big Delay Chain
        //reacts to trigger and increments score
        .Add(new IncrementScoreSystem(contexts))

        //reacts incremented score and updates visuals
        .Add(new RenderScoreSystem(contexts))

        //reacts to trigger && does a countdown before creating an entity + component
        .Add(new DelayResetPositionSystem(contexts))

        //reacts to the entity + component created by DelayResetPositionSystem && stops the ball velocity
        .Add(new HaltBallVelocitySystem(contexts))

        //reacts to the entity + component created by DelayResetPositionSystem && resets positions of rackets, ball, etc.
        .Add(new ResetGamePositionSystem(contexts))

        //starts another countdown that creates a new entity + component
        .Add(new DelayNewRoundSystem(contexts))

        //reacts on this new entity + component to make the ball move again
        .Add(new StartBallVelocitySystem(contexts))
        #endregion 

        #region Winner System Chain
        //detect when a player wins & winningPlayerEntity;
        .Add(new DetectWinningPlayer(contexts))

        //Detect winningPlayerEntity and create disableInputEntity
        .Add(new DisableMovementInputSystem(contexts))

        //Detect winningPlayerEntity and reset position of rackets & ball
        .Add(new PositionResetOnWinSystem(contexts))

        //Detect winningPlayerEntity and set ball velocity to 0
        .Add(new BallVelocityZeroSystem(contexts))

        //Detect winningPlayerEntity and initialize Panel GameObject
        .Add(new SpawnWinnerPanelSystem(contexts))

        //Detect winningPlayerEntity and initialize winningPlayerText GameObject
        .Add(new YouWinTextSystem(contexts))

        //Detect winningPlayerEntity and initialize QuitGameButton GameObject
        .Add(new QuitGameButtonSystem(contexts))

        //Detect winningPlayerEntity and initialize StartNewGameButton GameObject
        .Add(new StartNewGameButtonSystem(contexts))
        #endregion

        .Add(new SmallAi(contexts))
        .Add(new StartNewGameSystem(contexts))
        .Add(new ReInitializeBallVelocitySystem(contexts))

        //Cleanup
        .Add(new DestroySystem(contexts))

        ; //Semicolon to end feature

        return systems;
    }
}

public class CreateInitialGameStateSystem : IInitializeSystem
{
    GameContext _context;

    public CreateInitialGameStateSystem(Contexts contexts)
    {
        _context = contexts.game;
    }

    public void Initialize()
    {
        var topWall = _context.CreateEntity();
        topWall.AddAsset("WallHorizontal");
        topWall.AddPosition(0, 98.85f);
        topWall.isWall = true;

        var bottomWall = _context.CreateEntity();
        bottomWall.AddAsset("WallHorizontal");
        bottomWall.AddPosition(0, -98.7f);
        bottomWall.isWall = true;

        var leftWall = _context.CreateEntity();
        leftWall.AddPosition(-176.68f, 0);
        leftWall.AddAsset("WallVertical");
        leftWall.isWall = true;
        leftWall.isLeftWall = true;

        var rightWall = _context.CreateEntity();
        rightWall.AddPosition(176.68f, 0);
        rightWall.AddAsset("WallVertical");
        rightWall.isWall = true;
        rightWall.isRightWall = true;

        var dottedLine = _context.CreateEntity();
        dottedLine.AddPosition(0, 0);
        dottedLine.AddAsset("DottedLine");

        var player1 = _context.CreateEntity();
        player1.AddAsset("Racket");
        player1.AddPosition(-161.32f, 0);
        player1.isPlayer1 = true;

        var player2 = _context.CreateEntity();
        player2.AddAsset("Racket");
        player2.AddPosition(161.32f, 0);
        player2.isPlayer2 = true;
        player2.AddSpeed(1.5f);

        var ball = _context.CreateEntity();
        ball.AddAsset("Ball");
        ball.AddSpeed(100);
        ball.isBall = true;
        ball.AddInitialBallVelocity(new Vector2(4 ,1));

        var scoreLeft = _context.CreateEntity();
        scoreLeft.AddAsset("ScoreLeft");
        scoreLeft.isScoreLeft = true;
        scoreLeft.isText = true;

        var scoreRight = _context.CreateEntity();
        scoreRight.AddAsset("ScoreRight");
        scoreRight.isScoreRight = true;
        scoreRight.isText = true;

        var player1Score = _context.CreateEntity();
        player1Score.AddPlayer1Score(0);

        var player2Score = _context.CreateEntity();
        player2Score.AddPlayer2Score(0);

        _context.SetDelaySettings(1.5f ,1);



    }
}

public class SpawnPrefab : IInitializeSystem
{
    IGroup<GameEntity> assetGroup;
    GameContext gameContext;

    public SpawnPrefab(Contexts contexts)
    {
        assetGroup = contexts.game.GetGroup(GameMatcher.AllOf(GameMatcher.Asset).NoneOf(GameMatcher.Text));
        gameContext = contexts.game;
    }


    public void Initialize()
    {
        foreach (var e in assetGroup.GetEntities())
        {
            GameObject myPrefab = GameObject.Instantiate(Resources.Load(e.asset.myPrefabName)) as GameObject;
            e.AddView(myPrefab);
        }

    }
}

public class InstantiateUiSystem : IInitializeSystem
{
    GameContext gameContext;
    IGroup<GameEntity> textGroup;

    public InstantiateUiSystem(Contexts contexts)
    {
        gameContext = contexts.game;
        textGroup = gameContext.GetGroup(GameMatcher.Text);
    }

    public void Initialize()
    {
        foreach (var e in textGroup.GetEntities())
        {
            var ui = gameContext.uiRoot.root;

            GameObject textPrefabs = GameObject.Instantiate(Resources.Load(e.asset.myPrefabName), ui) as GameObject;
            e.AddView(textPrefabs);
        }
    }
}

public class SetWallsPosition : IInitializeSystem
{
    IGroup<GameEntity> groupWallEntities;

    public SetWallsPosition(Contexts contexts)
    {
        groupWallEntities = contexts.game.GetGroup(GameMatcher.AllOf(GameMatcher.Wall, GameMatcher.View, GameMatcher.Position));
    }

    public void Initialize()
    {
        foreach (var e in groupWallEntities.GetEntities())
        {
            var wallPosition = e.position;
            e.view.gameObject.transform.position = new Vector2(wallPosition.x, wallPosition.y);
        }
    }
}

public class InitializeScoreSystem : IInitializeSystem
{
    GameContext gameContext;
    IGroup<GameEntity> wallGroup;

    public InitializeScoreSystem(Contexts contexts)
    {
        gameContext = contexts.game;
        wallGroup = contexts.game.GetGroup(GameMatcher.AnyOf(GameMatcher.ScoreLeft, GameMatcher.ScoreRight));
    }

    public void Initialize()
    {
        foreach (var e in wallGroup.GetEntities())
        {
            var playerOneScore = e.isScoreLeft;
            var playerTwoScore = e.isScoreRight;

            if (playerOneScore)
            {
                var scoreEntity = gameContext.scoreLeftEntity;
                var playerOnecanvas = scoreEntity.view.gameObject;
                var canvasText = playerOnecanvas.GetComponent<Text>();

                canvasText.text = "Score: 0";
            }

            if (playerTwoScore)
            {
                var scoreEntity = gameContext.scoreRightEntity;
                var playerTwoCanvas = scoreEntity.view.gameObject;
                var canvasText = playerTwoCanvas.GetComponent<Text>();

                canvasText.text = "Score: 0";
            }
        }
    }
}

public class MovingBall : IInitializeSystem
{
    GameContext _context;


    public MovingBall(Contexts contexts)
    {
        _context = contexts.game;
    }

    public void Initialize()
    {
        if (_context.isBall && _context.hasInitialBallVelocity)
        {
            var ballEntity = _context.ballEntity;

            if (ballEntity.hasView)
            {
                var initialVelocity = _context.initialBallVelocity.value;

                var ballSpeed = _context.ballEntity.speed.value;

                var ballObject = ballEntity.view.gameObject;


                ballObject.GetComponent<Rigidbody2D>().velocity = initialVelocity * ballSpeed;
            }

        }
    }
}

public class RenderPosition : IExecuteSystem
{
    IGroup<GameEntity> _group;

    public RenderPosition(Contexts contexts)
    {
        _group = contexts.game.GetGroup(GameMatcher.AnyOf(GameMatcher.Player1, GameMatcher.Player2));
    }

    public void Execute()
    {
        foreach (var e in _group.GetEntities())
        {
            var getPosition = e.position;
            e.view.gameObject.transform.position = new Vector2(getPosition.x, getPosition.y);
        }
    }
}

public class GetInput : IExecuteSystem
{
    InputContext inputContext;

    public GetInput(Contexts contexts)
    {
        inputContext = contexts.input;
    }

    public void Execute()
    {
        if (!inputContext.isDisableInput)
        {
            //Player1 Movement
            float storeAxis = Input.GetAxisRaw("Vertical");
            inputContext.CreateEntity().AddverticalAxis(storeAxis);
        }

        if (!inputContext.isDisableInput)
        {
            //Change Player1 Color
            bool changeColor = Input.GetKeyDown(KeyCode.Space);

            if (changeColor)
            {
                inputContext.CreateEntity().isChangeColor = true;
            }
        }
    }
}

public class usePlayer1Input : ReactiveSystem<InputEntity>
{
    GameContext gameContext;

    public usePlayer1Input(Contexts contexts) : base(contexts.input)
    {
        //inputContext = contexts.input;
        gameContext = contexts.game;
    }

    protected override ICollector<InputEntity> GetTrigger(IContext<InputEntity> context)
    {
        return context.CreateCollector(InputMatcher.verticalAxis);
    }

    protected override bool Filter(InputEntity entity)
    {
        return entity.hasverticalAxis;
    }

    protected override void Execute(List<InputEntity> entities)
    {
        foreach (var e in entities)
        {

            //when getInput creates a new entity with verticalaxiscomponent in the input context
            //we want this reactive system to look for entities with verticalaxiscomponent in the input context
            //when verticalaxiscomponent is found we can use it to replace player1's position
            float verticalAxisInput = e.verticalAxis.value;
            var player1 = gameContext.player1Entity;
            float replaceYPos = player1.position.y + 2.5f * verticalAxisInput;
            replaceYPos = Mathf.Clamp(replaceYPos, -76.6f, 76.6f);
            player1.ReplacePosition(player1.position.x, replaceYPos);

            e.flagDestroy = true;
        }

    }
}

public class ChangeColorSystem : ReactiveSystem<InputEntity>
{
    GameContext context;

    public ChangeColorSystem(Contexts contexts) : base(contexts.input)
    {
        context = contexts.game;
    }

    protected override ICollector<InputEntity> GetTrigger(IContext<InputEntity> context)
    {
        return context.CreateCollector(InputMatcher.ChangeColor);
    }

    protected override bool Filter(InputEntity entity)
    {
        return entity.isChangeColor;
    }

    protected override void Execute(List<InputEntity> entities)
    {
        foreach (var e in entities)
        {
            if (context.isPlayer1)
            {
                var player1 = context.player1Entity;
                GameObject RacketP1 = player1.view.gameObject;

                float r, g, b;

                r = UnityEngine.Random.Range(0, 2);
                g = UnityEngine.Random.Range(0, 2);
                b = UnityEngine.Random.Range(0, 2);
                Color randomColor = new Color(r, g, b);
                RacketP1.GetComponent<SpriteRenderer>().color = randomColor;
            }

            e.flagDestroy = true;
        }
    }
}

public class SmallAi : IExecuteSystem
{
    GameContext _context;

    public SmallAi(Contexts contexts)
    {
        _context = contexts.game;
    }

    public void Execute()
    {
        //player 2 - The AI
        if (_context.isBall && _context.isPlayer2 && _context.ballEntity.hasView)
        {
            var player2 = _context.player2Entity;

            if (player2.hasPosition && player2.hasSpeed)
            {
                var player2PosX = player2.position.x;
                var player2PosY = player2.position.y;
                float speed = player2.speed.value;
                //ball
                var ballPosY = _context.ballEntity.view.gameObject.transform.position.y;

                //Calculate where AI has to go

                //Y Position difference of ball and AI 
                float deltaYPos = Mathf.Abs(ballPosY - player2PosY);
                int direction;//This causes the racket to shake

                //If ball y position is not too much different from AI
                if (deltaYPos < 5)
                {
                    direction = 0;
                }
                //if ball is above AI 
                else if (ballPosY > player2PosY)
                {
                    direction = 1;
                }
                else
                {
                    direction = -1;
                }



                float newYPos = (speed * direction) + player2PosY;

                newYPos = Mathf.Clamp(newYPos, -76.6f, 76.6f);

                //Replace position
                player2.ReplacePosition(player2PosX, newYPos);
            }

        }





    }
}

public class DetectTriggerSystem : ReactiveSystem<GameEntity>
{
    GameContext gameContext;
    IGroup<GameEntity> wallGroup;

    public DetectTriggerSystem(Contexts contexts) : base(contexts.game)
    {
        gameContext = contexts.game;
        wallGroup = gameContext.GetGroup(GameMatcher.AllOf(GameMatcher.View).AnyOf(GameMatcher.LeftWall, GameMatcher.RightWall));
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.Trigger); //think if we need to check for more that just a triggerComponent to avoid errors
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.hasTrigger;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var wall in wallGroup.GetEntities())
        {
            var wallObject = wall.view.gameObject;
            var triggerObject = entities[0].trigger.triggerOwner;

            if (wallObject == triggerObject)
            {

                //if this shit ain't working back to the double if statement
                var e = gameContext.CreateEntity();
                e.AddTriggerWallHit(wall.isLeftWall);

                e.flagDestroy = true;
            }
        }
    }
}

public class IncrementScoreSystem : ReactiveSystem<GameEntity>
{

    GameContext gameContext;

    public IncrementScoreSystem(Contexts contexts) : base(contexts.game)
    {
        gameContext = contexts.game;
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.TriggerWallHit);
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.hasTriggerWallHit;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var e in entities)
        {
            if (e.triggerWallHit.leftWall)
            {
                //incremet player2score
                int player2Points = gameContext.player2Score.points;
                gameContext.ReplacePlayer2Score(++player2Points);

            }
            else
            {
                //increment player1score
                int player1Points = gameContext.player1Score.points;
                gameContext.ReplacePlayer1Score(++player1Points);
            }


        }
    }
}

public class RenderScoreSystem : IExecuteSystem
{
    GameContext gameContext;
    ICollector<GameEntity> player1ScoreCollector;
    ICollector<GameEntity> player2ScoreCollector;


    public RenderScoreSystem(Contexts contexts)
    {
        player1ScoreCollector = contexts.game.CreateCollector(GameMatcher.Player1Score);
        player2ScoreCollector = contexts.game.CreateCollector(GameMatcher.Player2Score);
        gameContext = contexts.game;
    }

    public void Execute()
    {
        foreach (var e in player1ScoreCollector.collectedEntities)
        {
            if (e.hasPlayer1Score)
            {
                //add player1score.value to getcomponent<Text>.text = "Score : "+ player1Score.value
                var currentScore = e.player1Score.points;

                bool leftScroreExists = gameContext.isScoreLeft;
                bool leftScoreHasView = gameContext.scoreLeftEntity.hasView;

                //This ask if the context is scoreRight && if the scoreRightEntity has viewComponent
                if (leftScroreExists && leftScoreHasView)
                {
                    var scoreLeft = gameContext.scoreLeftEntity.view.gameObject;

                    scoreLeft.GetComponent<Text>().text = "Score: " + currentScore;
                }
            }
        }

        foreach (var e in player2ScoreCollector.collectedEntities)
        {
            if (e.hasPlayer2Score)
            {
                var currentScore = e.player2Score.points;

                bool rightScroreExists = gameContext.isScoreRight;
                bool rightScoreHasView = gameContext.scoreRightEntity.hasView;

                //This ask if the context is scoreRight && if the scoreRightEntity has viewComponent
                if (rightScroreExists && rightScoreHasView)
                {
                    var scoreRight = gameContext.scoreRightEntity.view.gameObject;

                    scoreRight.GetComponent<Text>().text = "Score: " + currentScore;
                }


            }
        }
        player1ScoreCollector.ClearCollectedEntities();
        player2ScoreCollector.ClearCollectedEntities();
    }
}

public class DelayResetPositionSystem : IExecuteSystem
{
    GameContext gameContext;
    IGroup<GameEntity> triggerGroup;

    public DelayResetPositionSystem(Contexts contexts)
    {
        gameContext = contexts.game;
        triggerGroup = contexts.game.GetGroup(GameMatcher.Trigger);
    }

    public void Execute()
    {
        foreach (var e in triggerGroup.GetEntities())
        {
            if (gameContext.hasDelaySettings)
            {
                if (!gameContext.hasTimeUntilResetPositions)
                {
                    float delay = gameContext.delaySettings.resetPositionDelay;
                    gameContext.SetTimeUntilResetPositions(delay);

                }
                float timeLeft = gameContext.timeUntilResetPositions.value;
                timeLeft -= Time.deltaTime;
                gameContext.ReplaceTimeUntilResetPositions(timeLeft);

                if (timeLeft <= 0)
                {
                    gameContext.isDelay = true;
                    e.flagDestroy = true;
                    gameContext.timeUntilResetPositionsEntity.flagDestroy = true;
                }
            }
            
        }
    }
}

public class HaltBallVelocitySystem : ReactiveSystem<GameEntity>
{
    GameContext gameContext;

    public HaltBallVelocitySystem(Contexts contexts) : base(contexts.game)
    {
        gameContext = contexts.game;
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.Delay);
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.isDelay;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var e in entities)
        {
            if (gameContext.ballEntity != null)
            {
                if (gameContext.ballEntity.hasView)
                {
                    var ballObject = gameContext.ballEntity.view.gameObject;

                    Debug.Log("stop moving ball.");
                    ballObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                }
            }

        }
    }
}

public class ResetGamePositionSystem : ReactiveSystem<GameEntity>
{
    GameContext gameContext;

    public ResetGamePositionSystem(Contexts contexts) : base(contexts.game)
    {
        gameContext = contexts.game;
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.Delay);
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.isDelay;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var e in entities)
        {
            //Reset player1
            if (gameContext.player1Entity != null)
            {
                if (gameContext.player1Entity.hasPosition)
                {
                    var player1PosX = gameContext.player1Entity.position.x;

                    gameContext.player1Entity.ReplacePosition(player1PosX, 0);
                }
            }

            //Reset player2
            if (gameContext.player2Entity != null)
            {
                if (gameContext.player2Entity.hasPosition)
                {
                    var player2posX = gameContext.player2Entity.position.x;

                    gameContext.player2Entity.ReplacePosition(player2posX, 0);
                }
            }

            if (gameContext.ballEntity != null)
            {
                if (gameContext.ballEntity.hasView)
                {
                    var ballObject = gameContext.ballEntity.view.gameObject;

                    ballObject.transform.position = Vector2.zero;
                }
            }
          
        }
    }
}

public class DelayNewRoundSystem : IExecuteSystem
{
    GameContext gameContext;

    public DelayNewRoundSystem(Contexts contexts)
    {
        gameContext = contexts.game;
    }

    public void Execute()
    {

        if (gameContext.isDelay && gameContext.hasDelaySettings)
        {
            if (!gameContext.hasTimeUntilNewRound)
            {
                float delay = gameContext.delaySettings.newRoundDelay;
                gameContext.SetTimeUntilNewRound(delay);
            }
            float timeLeft = gameContext.timeUntilNewRound.value;

            timeLeft -= Time.deltaTime;

            gameContext.ReplaceTimeUntilNewRound(timeLeft);

            if (timeLeft <= 0)
            {
                //Create entity
                gameContext.isDelayNewRound = true;

                //Destroy entity
                gameContext.delayNewRoundEntity.flagDestroy = true;

                gameContext.timeUntilNewRoundEntity.flagDestroy = true;

                gameContext.isDelay = false;
            }
        }



    }
}

public class StartBallVelocitySystem : ReactiveSystem<GameEntity>
{
    GameContext gameContext;

    public StartBallVelocitySystem(Contexts contexts) : base(contexts.game)
    {
        gameContext = contexts.game;
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.DelayNewRound);
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.isDelayNewRound;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var e in entities)
        {
            if (!gameContext.isPlayer1Winner && !gameContext.isPlayer2Winner)
            {
                var ballEntity = gameContext.ballEntity;
                //Do Something.
                if (ballEntity != null)
                {
                    if (ballEntity.hasView && ballEntity.hasInitialBallVelocity)
                    {
                        float ballSpeed = ballEntity.speed.value;

                        var ballObject = ballEntity.view.gameObject;
                        var ballRigidBody = ballObject.GetComponent<Rigidbody2D>();
                        Debug.Log("start moving ball again.");
                        ballRigidBody.velocity = ballEntity.initialBallVelocity.value * ballSpeed;
                        e.isDelayNewRound = false;
                    }
                }
            }

        }
    }
}

public class DetectWinningPlayer : ReactiveSystem<GameEntity>
{
    GameContext gameContext;

    public DetectWinningPlayer(Contexts contexts) : base(contexts.game)
    {
        gameContext = contexts.game;
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.AnyOf(GameMatcher.Player1Score, GameMatcher.Player2Score));
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.hasPlayer1Score || entity.hasPlayer2Score;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var e in entities)
        {
            if (gameContext.player1Score.points >= 10)
            {
                Debug.Log("Player 1 Winner!");
                var newEntity = gameContext.CreateEntity();
                newEntity.isPlayer1Winner = true;

                //newEntity.flagDestroy = true;
                
            }

            if (gameContext.player2Score.points >= 10)
            {
                Debug.Log("Player 2 Winner!");
                var newEntity = gameContext.CreateEntity();
                newEntity.isPlayer2Winner = true;

                //newEntity.flagDestroy = true;
            }
        }
    }
}

public class DisableMovementInputSystem : ReactiveSystem<GameEntity>
{
    InputContext inputContext;

    public DisableMovementInputSystem(Contexts contexts) : base(contexts.game)
    {
        inputContext = contexts.input;
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.AnyOf(GameMatcher.Player1Winner, GameMatcher.Player2Winner));
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.isPlayer1Winner || entity.isPlayer2Winner;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var e in entities)
        {
            inputContext.isDisableInput = true;
        }
    }
}

public class PositionResetOnWinSystem : ReactiveSystem<GameEntity>
{
    GameContext gameContext;

    public PositionResetOnWinSystem(Contexts contexts) : base(contexts.game)
    {
        gameContext = contexts.game;
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.AnyOf(GameMatcher.Player1Winner, GameMatcher.Player2Winner));
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.isPlayer1Winner || entity.isPlayer2Winner;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var e in entities)
        {
            //reset player 1 position
            if (gameContext.player1Entity != null)
            {
                if (gameContext.player1Entity.hasPosition)
                {
                    float player1PosX = gameContext.player1Entity.position.x;
                    gameContext.player1Entity.ReplacePosition(player1PosX, 0);
                }
            }

            //reset player 2 position
            if (gameContext.player2Entity != null)
            {
                if (gameContext.player2Entity.hasPosition)
                {
                    float player2PosX = gameContext.player2Entity.position.x;
                    gameContext.player2Entity.ReplacePosition(player2PosX, 0);
                }
            }

            //reset ball position
            if (gameContext.ballEntity != null)
            {
                if (gameContext.ballEntity.isBall)
                {
                    gameContext.ballEntity.ReplacePosition(0, 0);
                }
            }

        }
    }
}

public class BallVelocityZeroSystem : ReactiveSystem<GameEntity>
{
    GameContext gameContext;

    public BallVelocityZeroSystem(Contexts contexts) : base(contexts.game)
    {
        gameContext = contexts.game;
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.AnyOf(GameMatcher.Player1Winner, GameMatcher.Player2Winner));
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.isPlayer1Winner || entity.isPlayer2Winner;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var e in entities)
        {
            if (gameContext.ballEntity != null)
            {
                if (gameContext.ballEntity.hasVelocity)
                {
                    Debug.Log("Ballvelocity to ZERO BRO");
                    var ballVelocity = gameContext.ballEntity.velocity.value;
                    ballVelocity = new Vector2(0, 0);
                }
            }
        }
    }
}

public class SpawnWinnerPanelSystem : ReactiveSystem<GameEntity>
{
    GameContext gameContext;

    public SpawnWinnerPanelSystem(Contexts contexts) : base(contexts.game)
    {
        gameContext = contexts.game;
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.AnyOf(GameMatcher.Player1Winner, GameMatcher.Player2Winner));
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.isPlayer1Winner || entity.isPlayer2Winner;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var e in entities)
        {
            if (gameContext.uiRootEntity != null)
            {
                if (gameContext.uiRootEntity.hasUiRoot)
                {
                    var ui = gameContext.uiRoot.root;

                    GameObject panelPrefab = GameObject.Instantiate(Resources.Load("WinnerPanel"), ui) as GameObject;
                    var panel = gameContext.CreateEntity();
                    panel.AddView(panelPrefab);
                    panel.isUiElement = true;
                }
            }
        }
    }
}

public class YouWinTextSystem : ReactiveSystem<GameEntity>
{
    GameContext gameContext;

    public YouWinTextSystem(Contexts contexts) : base(contexts.game)
    {
        gameContext = contexts.game;
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.AnyOf(GameMatcher.Player1Winner, GameMatcher.Player2Winner));
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.isPlayer1Winner || entity.isPlayer2Winner;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var e in entities)
        {
            if (gameContext.uiRootEntity != null)
            {
                if (gameContext.uiRootEntity.hasUiRoot)
                {
                    var ui = gameContext.uiRoot.root;
                    GameObject winnerTextPrefab = GameObject.Instantiate(Resources.Load("WinnerText"), ui) as GameObject;

                    if (e.isPlayer1Winner)
                    {
                        winnerTextPrefab.GetComponent<Text>().text = "Winner\nPlayer1";
                    }

                    if (e.isPlayer2Winner)
                    {
                        winnerTextPrefab.GetComponent<Text>().text = "Winner\nPlayer2";
                    }

                    var winnerText = gameContext.CreateEntity();
                    winnerText.AddView(winnerTextPrefab);
                    winnerText.isUiElement = true;
                }
            }



        }
    }
}

public class QuitGameButtonSystem : ReactiveSystem<GameEntity>
{
    GameContext gameContext;

    public QuitGameButtonSystem(Contexts contexts) : base(contexts.game)
    {
        gameContext = contexts.game;
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.AnyOf(GameMatcher.Player1Winner, GameMatcher.Player2Winner));
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.isPlayer1Winner || entity.isPlayer2Winner;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var e in entities)
        {
            var ui = gameContext.uiRoot.root;

            GameObject quitGameButtonPrefab = GameObject.Instantiate(Resources.Load("QuitGameButton"), ui) as GameObject;
            var quitGameButton = gameContext.CreateEntity();
            quitGameButton.AddView(quitGameButtonPrefab);
            quitGameButton.isUiElement = true;
        }
    }
}

public class StartNewGameButtonSystem : ReactiveSystem<GameEntity>
{
    GameContext gameContext;

    public StartNewGameButtonSystem(Contexts contexts) : base(contexts.game)
    {
        gameContext = contexts.game;
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.AnyOf(GameMatcher.Player1Winner, GameMatcher.Player2Winner));
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.isPlayer1Winner || entity.isPlayer2Winner;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var e in entities)
        {
            var ui = gameContext.uiRoot.root;

            GameObject newGameButtonPrefab = GameObject.Instantiate(Resources.Load("NewGameButton"), ui) as GameObject;
            var newGameButton = gameContext.CreateEntity();
            newGameButton.AddView(newGameButtonPrefab);
            newGameButton.isNewGameButton = true;
            newGameButton.isUiElement = true;
        }
    }
}

public class StartNewGameSystem : IExecuteSystem
{
    GameContext gameContext;
    InputContext inputContext;
    IGroup<GameEntity> newGameButtonGroup;
    IGroup<GameEntity> buttonGroup;
    IGroup<GameEntity> scoreGroup;
    IGroup<GameEntity> uiGroup;
    IGroup<InputEntity> disableInputGroup;
    


    public StartNewGameSystem(Contexts contexts)
    {
        gameContext = contexts.game;
        inputContext = contexts.input;
        newGameButtonGroup = gameContext.GetGroup(GameMatcher.AllOf(GameMatcher.NewGameButton, GameMatcher.View));
        buttonGroup = gameContext.GetGroup(GameMatcher.NewGameButton);
        scoreGroup = gameContext.GetGroup(GameMatcher.AnyOf(GameMatcher.Player1Score, GameMatcher.Player2Score));
        uiGroup = gameContext.GetGroup(GameMatcher.UiElement);
        disableInputGroup = inputContext.GetGroup(InputMatcher.DisableInput);

    }

    public void Execute()
    {
        foreach (var e in newGameButtonGroup.GetEntities())
        {
            if (e.view.gameObject.GetComponent<Button>().enabled == false)
            {
                foreach (var button in buttonGroup.GetEntities())
                {
                    e.view.gameObject.GetComponent<Button>().enabled = true;
                    Debug.Log("Reactivate button");
                }

                foreach (var uiElement in uiGroup.GetEntities())
                {
                    Debug.Log("DESTROY!!!");
                    uiElement.view.gameObject.DestroyGameObject();
                    uiElement.Destroy();
                }

                foreach (var score in scoreGroup.GetEntities())
                {

                    if (score.hasPlayer1Score && !score.hasPlayer2Score)
                    {
                        gameContext.ReplacePlayer1Score(0);
                    }

                    if (score.hasPlayer2Score && !score.hasPlayer1Score)
                    {
                        gameContext.ReplacePlayer2Score(0);
                    }
                }

                gameContext.isPlayer1Winner = false;
                gameContext.isPlayer2Winner = false;


                foreach (var disabledInput in disableInputGroup.GetEntities())
                {
                    inputContext.isDisableInput = false;
                }
            }
        }
    }
}

public class ReInitializeBallVelocitySystem : ReactiveSystem<GameEntity>
{
    GameContext gameContext;

    public ReInitializeBallVelocitySystem(Contexts contexts) : base(contexts.game)
    {
        gameContext = contexts.game;
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.UiElement.Removed());
    }

    protected override bool Filter(GameEntity entity)
    {
        return !entity.isEnabled;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var e in entities)
        {
            var intialBallVelocity = gameContext.ballEntity.initialBallVelocity.value;
            var ballSpeed = gameContext.ballEntity.speed.value;

            var ballObject = gameContext.ballEntity.view.gameObject;
            var ballRigidBody = ballObject.GetComponent<Rigidbody2D>();

            ballRigidBody.velocity = intialBallVelocity * ballSpeed;
        }
    }
}
public class DestroySystem : ICleanupSystem
{
    IGroup<GameEntity> _gameDestroy;
    IGroup<InputEntity> _inputDestroy;

    public DestroySystem(Contexts contexts)
    {
        //contexts.game.GetGroup(Matcher<GameEntity>.AllOf(GameMatcher.Destroy).AnyOf(GameMatcher.Triangle, GameMatcher.Square));
        _gameDestroy = contexts.game.GetGroup(GameMatcher.Destroy);
        _inputDestroy = contexts.input.GetGroup(InputMatcher.Destroy);
    }

    public void Cleanup()
    {
        foreach (var e in _gameDestroy.GetEntities())
        {
            e.Destroy();
        }

        foreach (var e in _inputDestroy.GetEntities())
        {
            e.Destroy();
        }
    }
}
