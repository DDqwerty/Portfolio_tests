using NUnit.Framework;
using Altom.AltUnityDriver;

public class ChestCollectTest
{
    public AltUnityDriver tester;

    //Before any test it connects with the socket
    [OneTimeSetUp]
    public void SetUp()
    {
        tester = new AltUnityDriver();
    }

    //At the end of the test closes the connection with the socket
    [OneTimeTearDown]
    public void TearDown()
    {
        tester.Stop();
    }

    private void MoveTo(AltUnityVector3 current_position, AltUnityVector3 target_position)
    {
        if (current_position.y - target_position.y > 0.01f)
        {
            tester.KeyDown(AltUnityKeyCode.DownArrow);
            tester.KeyUp(AltUnityKeyCode.UpArrow);
        }

        if (current_position.y - target_position.y < 0.01f)
        {
            tester.KeyUp(AltUnityKeyCode.UpArrow);
            tester.KeyUp(AltUnityKeyCode.DownArrow);
        }

        if (current_position.y - target_position.y < -0.01f)
        {
            tester.KeyDown(AltUnityKeyCode.UpArrow);
            tester.KeyUp(AltUnityKeyCode.DownArrow);
        }

        if (current_position.x - target_position.x > 0.01f)
        {
            tester.KeyDown(AltUnityKeyCode.LeftArrow);
            tester.KeyUp(AltUnityKeyCode.RightArrow);
        }

        if (current_position.x - target_position.x < 0.01f)
        {
            tester.KeyUp(AltUnityKeyCode.LeftArrow);
            tester.KeyUp(AltUnityKeyCode.RightArrow);
        }

        if (current_position.x - target_position.x < -0.01f)
        {
            tester.KeyDown(AltUnityKeyCode.RightArrow);
            tester.KeyUp(AltUnityKeyCode.LeftArrow);
        }

        tester.PressKey(AltUnityKeyCode.Space);

        UnityEngine.Debug.Log(current_position.x);
        UnityEngine.Debug.Log(target_position.x);
    }

    [Test]
    public void Test()
    {

        tester.LoadScene("Main");
        tester.FindObject(By.NAME, "GameManager").CallComponentMethod("GameManager", "NewGame", "");
        tester.FindObject(By.NAME, "Player").SetComponentProperty("Player", "maxhitPoints","1000");
        tester.FindObject(By.NAME, "Player").SetComponentProperty("Player", "hitPoints", "1000");

        int collected = 0;

        while (collected<5)
        {
            //var fighters = tester.FindObjects(By.TAG, "Fighter");

            //foreach (AltUnityObject fighter in fighters)
            //{
            //    if (fighter.name != "Player")
            //        fighter.CallComponentMethod("UnityEngine.GameObject", "SetActive", "false");
            //}

            var chest = tester.FindObjectsWhichContain(By.NAME,"Chest");

            while(chest.Count>0)
            {
                UnityEngine.Debug.Log(chest[0].name);
                if (chest[0].GetComponentProperty("Chest", "collected") == "false")
                {
                    MoveTo(tester.FindObject(By.NAME, "Player").getWorldPosition(), chest[0].getWorldPosition());
                }
                else
                {
                    chest[0].CallComponentMethod("UnityEngine.GameObject", "SetActive", "false");
                    chest.RemoveAt(0);
                    collected++;
                }
            }

            MoveTo(tester.FindObject(By.NAME, "Player").getWorldPosition(), tester.FindObject(By.NAME,"Exit").getWorldPosition());
        }

        Assert.IsTrue(collected >= 5);
    }
}