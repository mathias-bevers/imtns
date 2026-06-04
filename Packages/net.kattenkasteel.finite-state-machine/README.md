- [Introduction](#introduction)
- [Components](#components)
- [State Machine](#state-machine)
- [State](#state)
- [Condition](#condition)
- [Transition](#transition)
- [Transitioner](#transitioner)


# Introduction
This is a finite state machine package, using Unity's scriptable objects as a base.

# Components
This has four main components, the state machine, a state, a condition, and a transition.

## State Machine
The state machine is a signleton that handles the current state, state transitions and checking the transitions'
conditions. The transtions of states work on request bases.

## State
A state is scriptable object that holds the basic information of a state. By default it holds the state's name, scene 
index, wheter it is a parent, and a child collection In this implementation there are two types of states, a 
**_parent_** and a **_child_** state. A **_child_** state can be completed on it own, it does not concern it self with 
other states. A **_parent_** state has an collection of **_child_** states, it is concidered complete if all **_child_**
states are completed. 

## Condition
A condition is an abstact scriptable object, it holds an error message. This message is thrown as an exception when the 
condition is not met when trying to transition. This scriptable object is serving as a base to create own coditions 
*(e.g. player health, state completion, etc)* by default it has access to the calling transition. 

## Transition
A transition is a class that holds a state and a collection of conditions. If the transition is called and all the 
conditions in the collection are met, the state machine will switch to the target state.

### Transitioner
The transitioner class is a container to hold a transition. This is allows it to be used with unity events like a button 
click.