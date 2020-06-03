function MyModule::create( %this )
{
    // Load GUI profiles.
    exec("./gui/guiProfiles.cs");
    exec("./scripts/scenewindow.cs");
    exec("./scripts/scene.cs");
    exec("./scripts/controls.cs");

    exec("./scripts/background.cs");
    exec("./scripts/spaceship.cs");
    exec("./scripts/asteroids.cs");

    createSceneWindow();
    
    createScene();
    mySceneWindow.setScene(myScene);

    createBackground();

    createSpaceShip();

    createAsteroids(20);  

    //Scrpt manager named InputManager
    new ScriptObject(InputManager);
    mySceneWindow.addInputListener(InputManager);

    InputManager.Init_controls();

    // //Enable the debug visualization for "collision", "position" and "aabb"
    // myScene.setDebugOn("collision", "position", "aabb");
    // //Disable the debug visualization for "collision", "position" and "aabb"
    // myScene.setDebugOff("collision", "position", "aabb");
}

function MyModule::destroy( %this )
{
    //mySceneWindow.removeInputListener(InputManager); //Disable input listner
    InputManager.delete();
    shipcontrols.pop();
    destroySceneWindow();
}