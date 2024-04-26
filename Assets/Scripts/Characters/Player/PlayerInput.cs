using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInput : MonoBehaviour
{

    public float Vertical { get; private set; }
    public float Horizontal { get; private set; }
    public bool Attack { get; private set; }

    private readonly string[] Axises =
    {
        "Vertical",
        "Horizontal",
        "Attack"
    };

    public event System.Action OnMove;

    private void Update()
    {
        if (GameManager.Instance.IsPaused)
            return;

        Vertical = Input.GetAxis(Axises[0]);
        Horizontal = Input.GetAxis(Axises[1]);
        Attack = Input.GetButton(Axises[2]);

        if(OnMove != null && Vertical + Horizontal > 0)
        {
            OnMove();
        }
    }
}
