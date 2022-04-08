using Buildings;
using Core;

public class Rocket : Building
{
    public void LaunchRocket()
    {
        if (_population > 0)
        {
            GameManager.Instance.Evacuate(_population);
            _population = 0;
            AudioManager.Instance.PlaySFX("launch");
            Delete();
        }
    }
}