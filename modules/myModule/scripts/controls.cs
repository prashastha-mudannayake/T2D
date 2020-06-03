function InputManager::onTouchDown(%this, %touchId, %worldposition)
{    

//We assign the list of objects that were hit to variable %picked
%picked = myScene.pickPoint(%worldposition);

//%picked.count is the number of items listed in the %picked variable

for(%i=0; %i<%picked.count; %i++)
   {
   
    //When iterating through the list, getWord will return item number %i in variable %picked
   
      %myobj = getWord(%picked,%i);
      
    //If this item belongs to SceneGroup 20, we delete it
      if(%myobj.getSceneGroup() == 20)
      {
      %myobj.safedelete();
      }
    
   }   
}

function InputManager::Init_controls(%this)
{
//Create our new ActionMap
new ActionMap(shipcontrols);

// Press "a" to execute "PlayerShip::turnleft();"
// Release "a" to execute "PlayerShip::stopturn();"

shipcontrols.bindCmd(keyboard, "a", "PlayerShip.turnleft();", "PlayerShip.stopturn();");
shipcontrols.bindCmd(keyboard, "d", "PlayerShip.turnright();", "PlayerShip.stopturn();");
shipcontrols.bindCmd(keyboard, "w", "PlayerShip.accelerate();", "PlayerShip.stopthrust();");

//Push our ActionMap on top of the stack
shipcontrols.push();
}