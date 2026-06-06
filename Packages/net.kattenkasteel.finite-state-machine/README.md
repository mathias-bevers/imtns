- [Introduction](#introduction)
- [Components](#components)
  - [State Machine](#state-machine)
  - [State](#state)
  - [Condition](#condition)
  - [Transition](#transition)
    - [Transitioner](#transitioner)

# Introduction
This is a finite state machine package, using Unity’s scriptable objects as a
base. The state machine is designed primarily for scene management.

# Components
This has four main components: the state machine, a state, a condition, and a
transition.

## State Machine
The state machine is a singleton that handles the current state, state
transitions, and checks the transitions’ conditions. The transitions of states
work on a request basis.

### State Machine API
a scriptable object made to basic calls like resetting or completing the current
active state

## State
A state is a scriptable object that holds the basic information of a state.
By default, it holds the state’s name, scene index, whether it is a parent, and
a child collection. In this implementation, there are two types of states: a
parent state and a child state. A child state can be completed on its own; it 
does not concern itself with other states. A parent state has a collection of 
child states; it is considered complete if all child states are completed.

## Condition
A condition is an abstract, scriptable object that holds an error message. This 
message is thrown as an exception when the condition is not met when trying to
transition. This scriptable object is serving as a base to create custom 
conditions, *(e.g. player health, state completion, etc)*. By default, a 
condition has access to the calling transition. 

## Transition
A transition is a class that holds a state and a collection of conditions. If 
the transition is called and all the conditions in the collection are met, the 
state machine will switch to the target state.

### Transitioner
The transitioner class is a container to hold a transition. This is allows it 
to be used with unity events, like a button click.