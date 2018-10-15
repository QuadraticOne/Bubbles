# Bubbles

Bubbles is a Unity implementation of an experimental way of thinking about game design that attempts to move away from the convention of a game loop.  The idea is to wrap each of the objects in the game in a "bubble", which has a few main properties:

1. The state of an object inside a bubble can be calculated as an explicit function of time up until a certain point, somewhere in the future, at which it becomes discontinuous.
2. The object contained within the bubble will only have measurable effects on a certain region of space, which may grow as the object moves or otherwise changes.  The size of the bubble's region of influence is expressed as a monotnically increasing function of time.

In theory, these two properties mean that a game can be defined in terms of the behaviour of an individual object over time, as well as a set of defined interactions between pairs of types of object.  This may lead to a decrease in bugs and abstracts game object design away from the update loop, as all bubbles can also act as game objects (in Unity's case, instances of `MonoBehaviour`) since their state can be evaluated as a function of time.  Because each object's region of influence is also defined, the game engine can defer calculating the effects of interactions between objects until it is strictly necessary.
