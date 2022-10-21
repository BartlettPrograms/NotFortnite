A more user friendly doc is available at http://nice-touch-docs.moremountains.com/
Find out more about the asset at http://nice-touch.moremountains.com/

Nice Touch v2.0

## WHAT'S IN THE ASSET ?
-------------------------

The asset contains two main folders : Common and Demos. As the name implies, Common contains all the scripts and visual assets necessary for the mobile controls to work. You'll want to keep that folder in your game. All the scrips are grouped in a Scripts/MMTools folder. That's because the scripts at the core of Nice Touch are part of MMTools, a bigger library used (among other places) in the Corgi Engine and in the Infinite Runner Engine.

Demos contains two demo scenes. NiceTouchTestScene is an exhaustive debug scene. It'll trigger console or on screen logs for every possible event (joystick pushed in a direction, button pressed, released, etc...). It can be useful if you're wondering how to bind a certain event. CubeCylinderSphere (I'm good at names) is a very, very minimal game, where you control three different "characters" using the Nice Touch controls. It's a basic but good example of how you can integrate these controls into your game. The demo folders are not required for the controls to work, you can remove them safely.


## HOW DO I ADD THIS TO MY GAME ?
------------------------------

If you're starting a brand new game, or if you don't have a GUI in your scene right now, the fastest way is to just drag and drop the UICamera prefab (located in Common/Resources/) into your scene. If you already have a GUI, that's also the fastest way, but you'll want to drag that prefab into the scene, unfold it and just take its MMControls part, and put it into your existing canvas.

Then, you'll (potentially) want to reposition the controls to your liking. They're built using Unity's uGUI best practices, if you're not familiar with it, there's tons of info over at Unity's website. Basically you can just move stuff around like you would any other object. You'll also probably want to remove some controls (who needs two joysticks and a D-pad ?).

Now, all that's left to do is bind these controls to your game.
The Nice Touch controls target functions, methods from your game's classes. Usually, you'll want to target either an InputManager class, that handles all your existing (keyboard or gamepad) input, or maybe target your characters directly. The first method was chosen for the Corgi Engine integration, and there's an example of the second one in the CubeCylinderSphere demo scene.
In any case, the main idea is the same. Your controls have to be linked to a method. When they're interacted with, they'll trigger this (or these) method(s).

This means you'll need clean methods on your game's side. For example, here are the methods on my test CharacterMovement class. They're extremely basic, but you'll get the idea :

public virtual void Move(Vector2 newMovement)
{
  if (!_axisBased)
  {
    _horizontalMovement = newMovement.x;
    _verticalMovement = newMovement.y;
  }
}

public virtual void Jump()
{
  _rigidbody.AddForce(Vector3.up * (JumpForce * -Physics.gravity.y));
}

As you can see, calling the Jump method (once) will make the character go up in the air, and passing a Vector2 to the Move method will modify the horizontal and vertical movement values of the character, which will then be applied to its position at Update(). Now if I want to have a button trigger the jump for that character everytime it's pressed for the first time (before a release), here's what I have to do :

- In the button's inspector, in the MMTouchButton component part, in the ButtonPressedFirstTime() box, select my character in the Object property
- In the newly populated dropdown of that same box, select my character's class, and then my Jump method
- That's it.
As you can see, there's not much to it. Here are a few remarks :

- Joysticks and arrows require "dynamic" methods. Which is Unity's way of naming methods that take one (or more) parameters. Joysticks will need a method that takes a Vector2 as a parameter, and Arrows will require a method with a float parameter.
These "dynamic" methods will always be at the very top when you open your character's class dropdown from the inspector
- For joysticks, you need to select the joystick knob to make changes, the upper level gameobject is just the "base" of your joystick, and is purely cosmetic, get rid of it if you want
- For joysticks, a Target Camera must be set. If you've just drag and dropped and are using the UICamera prefab, there's nothing to do. If you've changed the main UI camera, you'll just need to specify it (it's the first item in the MMTouchJoystick's inspector)
For all the controls, from the inspector, you can change the pressed opacity, this is the opacity that will be applied when the control is interacted with


## HOW DOES PLATFORM DETECTION WORK ?
-------------------------------------

If you look at the Hierarchy panel, you'll see that all the controls are nested under a MMTouchControls gameobject. This gameobject is basically just a CanvasGroup with a MMTouchControls component. It's this component that handles (if you want to use it) mobile detection. It works in a very simple way : if you're targeting a mobile platform (iOS or Android), it'll show the controls when you press play. If you're targeting another platform, it'll hide them. You can also force one mode or the other from the inspector.

## IS THERE DOCUMENTATION SOMEWHERE ?
-------------------------------------

There is!
There's a complete API documentation at http://nice-touch-docs.moremountains.com/API/
And a functional documentation at http://nice-touch-docs.moremountains.com/

## I STILL HAVE A QUESTION!
---------------------------

If something's still not clear, you can always drop me a line using the form at http://nice-touch-docs.moremountains.com/. It's entirely possible that I forgot to describe something in this page, but please make sure you've read it all before filling this form. You can also please check the FAQ before sending me an email. Chances are, your question's answered right there. If it's not, then go ahead!
Also, if you're asking for support, please send me your invoice number, along with your Unity version and the version of Nice Touch you're using, so I can help you best.
