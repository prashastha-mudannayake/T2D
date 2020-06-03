function createSpaceShip()
{
   // Create the sprite.
    %spaceship = new Sprite(PlayerShip);
    
    // We want our spaceship to move and be affected by gravity and various forces
    // so we set its BodyType to dynamic
    %spaceship.setBodyType( dynamic );
       
    // Set the position at the center of our Scene
    %spaceship.Position = "0 0";

    // Set the size.        
    %spaceship.Size = "4 4";
    
    // Set the layer closest to the camera (above the background)
    %spaceship.SceneLayer = 1;
    
    // Set the scroller to use an animation!
    %spaceship.Image = "myModule:LoRez_SpaceShip";

    // This creates a box which so that collisions with the screen edges register properly
    // Calling createPolygonBoxCollisionShape() without arguments sets the box to the size of the 
    // sceneobject automatically.
    %spaceship.createPolygonBoxCollisionShape();

    // Add the sprite to the scene.
    myScene.add( %spaceship );   

    %spaceship.setCollisionCallback( true );

    //setFixedAngle prevents the spaceship's rotation from being influenced by Collisions and physics forces
    //The spaceship will still bounce when it collides with other objects.
    %spaceship.setFixedAngle(true);

    //create a new variable and set it to false.
    %spaceship.isThrusting = false;
   
}

function PlayerShip::onCollision(%this, %sceneobject, %collisiondetails)
{
    //If we have collided with an object belonging to Scenegroup 20,
    //execute the code between the { ... }
    //If we collide with something else, do nothing
  if(%sceneobject.getSceneGroup() == 20)
  {
    // ParticlePlayer is also derived from SceneObject, we add it just like we've added all the other
    //objects so far
    %explosion = new ParticlePlayer();

    //We load the particle asset from our ToyAssets module
    %explosion.Particle = "ToyAssets:impactExplosion";

    //We set the Particle Player's position to %Sceneobject's position
    %explosion.setPosition(%sceneobject.getPosition());

    //This Scales the particles to twice their original size
    %explosion.setSizeScale(2);

    //When we add a Particle Effect to the Scene, it automatically plays
    myScene.add(%explosion);

    //We delete the asteroid
    %sceneobject.safedelete();
    
    //We create a new asteroid just like we did at the start of the game!
    createAsteroids(1);  
  }
}

function PlayerShip::accelerate(%this)  
{  
    //Get the angle of our spaceship. When the ship is pointing upwards, its Angle is 90.  
    %adjustedAngle = %this.Angle + 90;  
  
    //When used as a math operand, % refers to modulo (or modulus) operator  
    //This function can be read as %adjusted angle = %adjustedAngle % 360;  
    %adjustedAngle %= 360;  
  
    //If we are thrusting, shorten our vector  
    if(%this.isThrusting)  
    {    
        //Calculate a direction from an Angle and Magnitude  
        %ThrustVector= Vector2Direction(%adjustedAngle,35);  
    }  
    else  
    {  
        %ThrustVector = Vector2Direction(%adjustedAngle,95);  
  
        //We temporarily remove the Damping of Linear Velocity to allow full power!  
        %this.setLinearDamping(0.0);  
  
        //We temporarily increase the Damping of Angular velocity so that the ship turns slower when at full thrust  
        %this.setAngularDamping(2.0);  
    }  
  
    //Adding our position to the ThrustVector determines the strength of our thrust  
    %MywordX = %this.Position.x + %ThrustVector.x;  
    %MywordY = %this.Position.y + %ThrustVector.y;    
  
    //applyLinearImpulse pushes on our spaceship, using %ThrustVector as the impulse vector.  
    //The second parameter is the point in the ship's collision shape used to apply the thrust  
    %this.applyLinearImpulse(%ThrustVector, "0 0");  
  
    //We are now thrusting, we will set this to false when we release the 'w' key  
    %this.isThrusting = true;  
     
    //We create a schedule to repeat this thrust every 100 milliseconds  
    %this.thrustschedule = %this.schedule(100,accelerate);     
}

function PlayerShip::turnleft(%this)
{
//adds the value of 20 to our current Angular Velocity
%this.setAngularVelocity(%this.getAngularVelocity()+ 20);
%this.turnschedule = %this.schedule(100,turnleft);
}

function PlayerShip::turnright(%this)
{
//substracts the value of 20 from our current Angular velocity
%this.setAngularVelocity(%this.getAngularVelocity()- 20);

%this.turnschedule = %this.schedule(100,turnright);
}

function PlayerShip::stopturn(%this)
{
//cancels all scheduled turning
   cancel(%this.turnschedule);
//Stop us from spinning
   %this.setAngularVelocity(0);
}

function PlayerShip::stopthrust(%this)
{ 
//We add Damping to the Linear Velocity, which slows down the ship when the key is released
%this.setLinearDamping(0.8);
//We set Angular Damping to 0 so that we can turn as fast as possible
%this.setAngularDamping(0.0);

cancel(%this.thrustschedule);

//we set isThrusting to false to indicate that we are no longer thrusting.
//Next time we hit 'w', our accelerate function will use a bigger acceleration boost to get us going faster!
%this.isThrusting = false;
}