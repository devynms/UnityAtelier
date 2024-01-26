
namespace Atelier.Actors {

    /// <summary>
    /// Used to create a state machines for various game entities.
    /// 
    /// Typically, you create several private inner classes for the state machine and its states for
    /// a given controller. I can't remember if this is necessary because of the self-referential
    /// aspect of some of this code, or if it was just my stylistic choice.
    /// 
    /// Instead of creating a new object for a specific state each time you want to transition to
    /// that state, the intention here is to re-use a single instance of each state type.
    /// 
    /// Since the states are being re-used, the "PreparedState" wrapper was created to indicate
    /// that a state has been properly re-initialized, and isn't stale. All its doing is adding
    /// this constraint to the type system, since you're not calling new State() every time.
    /// 
    /// States that don't need to be initialized (or "prepared") can inherit from SimpleState.
    /// 
    /// The state machine, when passed a new state, can take PreparedState or SimpleState. It
    /// can't take a normal State instance directly.
    /// 
    /// Each state implementation has to forward the controller instance through the base
    /// constructor.
    /// 
    /// Since <c>this</c> will reference the code itself, the base constructor sets the controller
    /// instance to the <c>self</c> field.
    /// 
    /// So <c>this</c> == the state,
    /// and <c>self</c> == the actual controller.
    /// 
    /// This example shows how to assemble a state machine (with some code omitted).
    /// 
    /// <example>
    /// <code>
    /// namespace MyGame {
    ///     
    ///     class EnemyController {
    ///     
    ///         private EnemyStateMachine stateMachine;
    ///         private Idle idle;
    ///         private Pathing pathing;
    ///         private Attacking attacking;
    ///         
    ///         private void Awake() {
    ///             this.stateMachine = new EnemyStateMachine();
    ///             this.idle = new Idle(this);
    ///             this.pathing = new Pathing(this);
    ///             this.attacking = new Attacking(this);
    ///         }
    ///         
    ///         private void Start() {
    ///             this.stateMachine.Start(this.idle);
    ///         }
    ///         
    ///         private void Update() {
    ///             this.stateMachine.Update();
    ///         }
    ///         
    ///         private void FixedUpdate() {
    ///             this.stateMachine.CheckInput();
    ///             this.stateMachine.FixedUpdate();
    ///         }
    ///     
    ///         #region State Machine
    ///         
    ///         class EnemyStateMachine : StateMachine{SomethingController} {
    ///         }
    ///     
    ///         class Idle : SimpleState{EnemyController} {
    ///             public Idle(SomethingController controller) : base(controller) { }
    ///         }
    ///     
    ///         class Pathing : State{EnemyController} {
    ///             public Pathing(EnemyController controller) : base(controller) { }
    ///             
    ///             private const float SqrCloseEnough = 0.01f;
    ///             
    ///             public Vector2 target;
    ///             
    ///             public PreparedState{EnemyController} Prepare(Vector2 target) {
    ///                 this.target = target;
    ///                 return PreparedState{EnemyController}.Of(this);
    ///             }
    ///             
    ///             public override void Enter() {
    ///                 self.walkAudio.enabled = true;
    ///             }
    ///             
    ///             public override void Exit() {
    ///                 self.walkAudio.enabled = false;
    ///             }
    ///             
    ///             public override void FixedUpdate() {
    ///                 Vector2 position = self.body.Position;
    ///                 Vector2 delta = target - position;
    ///                 if (delta.sqrMagnitude <= SqrCloseEnough) {
    ///                     self.stateMachine.ChangeState(self.idle);
    ///                 } else {
    ///                     Vector2 motion = Vector2.ClampMagnitude(delta, self.speed) * Time.deltaTime;
    ///                     self.body.MoveAndSlide(motion);
    ///                 }
    ///             }
    ///         }
    ///         
    ///         class Attacking : State{EnemyController} {
    ///         
    ///             public Attacking(EnemyController controller) : base(controller) { }
    ///             
    ///             private int frameCount;
    ///             private Vector2 target; 
    ///             
    ///             public PreparedState{EnemyController} Prepare(Vector2 target) {
    ///                 this.target = target;
    ///                 return PreparedState{EnemyController}.Of(this);
    ///             }
    ///             
    ///             public override void Enter() {
    ///                 this.frameCount = 0;
    ///                 this.CreateAttack();
    ///                 self.StartAttackCooldown();
    ///                 self.attackPanel.PlayClip();
    ///             }
    ///             
    ///             public override void Exit() {
    ///                 this.RemoveAttack();
    ///             }
    ///             
    ///             public override void FixedUpdate() {
    ///                 this.frameCount += 1;
    ///                 if (this.frameCount == self.attackFrames) {
    ///                     self.stateMachine.ChangeState(self.idle);
    ///                 }
    ///             }
    ///             
    ///             private void CreateAttack() {
    ///                 ...
    ///             }
    ///             
    ///             private void RemoveAttack() {
    ///                 ...
    ///             }
    ///             
    ///         }
    ///         
    ///         # endregion
    ///     
    ///     }
    /// }
    /// </code>
    /// </example>
    /// 
    /// It's also possible to implement a sort of two-tier state machine system. I've done this to
    /// have "behaviors" on one hand (like wandering, tracking, investigating sound), and more 
    /// mechanical states on the other. (The intention here is that behaviors act like the "brain" 
    /// or AI of non-player actors, but can only control the mechanical state machine; while the
    /// mechanical state machine is the only thing that interacts directly with the outside world.)
    /// 
    /// The simplest way to do this is to simply... implement two state machines.
    /// 
    /// For an additional layer of type-safety (to ensure that behavior states don't get mixed up with
    /// mechanical states), you could create new base classes for each type of state:
    /// 
    /// <example>
    /// <code>
    /// 
    /// private sealed class BehaviorMachine : StateMachine{EnemyController} { 
    /// }
    /// 
    /// private abstract class Behavior : State{EnemyController} {
    ///     public Behavior(EnemyController controller) : base(controller) { }
    /// }
    /// 
    /// private abstract class SimpleBehavior : SimpleState{EnemyController} { 
    ///     public SimpleBehavior(EnemyController controller) : base(controller) { }
    /// }
    /// 
    /// private sealed class Wandering { ... }
    /// private sealed class Investigating { ... }
    /// private sealed class Following { ... }
    /// private sealed class Stunned { ... }
    /// 
    /// private sealed class EnemyStateMachine : StateMachine{EnemyController} {
    /// }
    /// 
    /// private abstract class EnemyState : State{EnemyController} {
    ///     public EnemyState(EnemyController controller) : base(controller) { }
    /// }
    /// 
    /// private abstract class SimpleEnemyState : SimpleState{EnemyController} { 
    ///     public SimpleEnemeyState(EnemyController controller) : base(controller) { }
    /// }
    /// 
    /// private sealed class Idle { ... }
    /// private sealed class Pathing { ... }
    /// private sealed class Attacking { ... }
    /// 
    /// </code>
    /// </example>
    /// 
    /// </summary>
    /// <typeparam name="ControllerType"></typeparam>
    public class StateMachine<ControllerType> {

        public State<ControllerType> CurrentState { get; private set; }


        /// <summary>
        /// Start the machine with an initial state.
        /// </summary>
        /// <param name="initialState">The initial state.</param>
        public void Start(PreparedState<ControllerType> initialState) {
            this.CurrentState = initialState;
            this.CurrentState.Enter();
        }

        /// <summary>
        /// Check for inputs or events that might change the current state on this frame.
        /// </summary>
        public void CheckInput() {
            this.CurrentState.CheckInput();
        }

        /// <summary>
        /// Execute gameplay logic for this state.
        /// </summary>
        public void FixedUpdate() {
            this.CurrentState.FixedUpdate();
        }

        /// <summary>
        /// Execute frame-update logic for this state.
        /// </summary>
        public void Update() {
            this.CurrentState.Update();
        }

        /// <summary>
        /// Change state.
        /// </summary>
        /// <param name="state">Next state.</param>
        public void ChangeState(PreparedState<ControllerType> state) {
            this.CurrentState.Exit();
            this.CurrentState = state;
            this.CurrentState.Enter();
        }
    }

    public struct PreparedState<ControllerType> {

        public State<ControllerType> state;

        public static PreparedState<ControllerType> Of(State<ControllerType> state) {
            return new PreparedState<ControllerType> { state = state };
        }


        public static implicit operator State<ControllerType>(PreparedState<ControllerType> self) => self.state;
    }

    public abstract class State<ControllerType> {

        private ControllerType controller;

        protected ControllerType self => this.controller;

        public State(ControllerType controller) {
            this.controller = controller;
        }

        /// <summary>
        /// Check for any inputs or events that might change the current state on this frame.
        /// </summary>
        public virtual void CheckInput() { }

        /// <summary>
        /// Enter the state, performing any setup logic.
        /// </summary>
        public virtual void Enter() { }

        /// <summary>
        /// Exit the state, performing any teardown logic.
        /// </summary>
        public virtual void Exit() { }

        /// <summary>
        /// Execute gameplay logic for this state.
        /// </summary>
        public virtual void FixedUpdate() { }

        /// <summary>
        /// Execute frame-update logic for this state.
        /// </summary>
        public virtual void Update() { }

    }

    public abstract class SimpleState<ControllerType> : State<ControllerType> {

        public SimpleState(ControllerType controller) : base(controller) {}

        public static implicit operator PreparedState<ControllerType>(SimpleState<ControllerType> self) => 
            PreparedState<ControllerType>.Of(self);

    }

}

