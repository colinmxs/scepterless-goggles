using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    public static SelectionManager instance = null;    
    CharacterController selected;

    //Awake is always called before any Start functions
    void Awake()
    {
        //Check if instance already exists
        if (instance == null)

            //if not, set instance to this
            instance = this;

        //If instance already exists and it's not this:
        else if (instance != this)

            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);

        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            CheckForDeselect();
        }

        if (Input.GetMouseButtonDown(1))
        {
            Move();
        }
    }

    void Move()
    {   
        var screenToWorldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        selected.Move(new Vector2(screenToWorldPoint.x, screenToWorldPoint.y));
    }

    void CheckForDeselect()
    {
        if (selected != null)
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if (!hit)
            {
                this.selected.Deselect();
                this.selected = null;
            }
        }
    }

    public void OnSelect(CharacterController selected)
    {
        if (this.selected != null && selected != this.selected)
        {
            this.selected.Deselect();
        }

        this.selected = selected;
    }
}
