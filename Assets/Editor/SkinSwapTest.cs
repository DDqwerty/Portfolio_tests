using NUnit.Framework;
using Altom.AltUnityDriver;

public class SkinSwapTest
{
    public AltUnityDriver tester;
    //Before any test it connects with the socket
    [OneTimeSetUp]
    public void SetUp()
    {
        tester =new AltUnityDriver();
    }

    //At the end of the test closes the connection with the socket
    [OneTimeTearDown]
    public void TearDown()
    {
        tester.Stop();
    }

    private void ClickOn(AltUnityVector2 position)
    {
        tester.MoveMouseAndWait(position,1);
        System.Threading.Thread.Sleep(1000);
        tester.PressKeyAndWait(AltUnityKeyCode.Mouse0,1);
        System.Threading.Thread.Sleep(1000);
    }

    private AltUnityVector2 Position(AltUnityObject altObject)
    {
        return (altObject.getScreenPosition());
    }

    [Test]
    public void Test()
    {
        tester.LoadScene("Main");
        tester.FindObject(By.NAME, "GameManager").CallComponentMethod("GameManager","NewGame","");

        var skin = tester.FindObject(By.NAME, "Player").GetComponentProperty("Player", "currentcharacter");

        string[] click_names = {
            "Menubutton",
            "Leftarrow",
        };

        foreach (string name in click_names)
        {
            AltUnityVector2 vector = Position(tester.FindObject(By.NAME,name));
            ClickOn(vector);
        }

        tester.FindObject(By.NAME, "Menu").CallComponentMethod("UnityEngine.Animator", "SetTrigger", "hide");

        Assert.IsTrue(tester.FindObject(By.NAME, "Player").GetComponentProperty("Player", "currentcharacter") != skin);
    }

}