using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    public static SelectionManager instance = null;
    Selectable selected;
    //public SelectionMarker marker;


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

        //marker.enabled = false;
    }


    void Update()
    {
        if (selected != null)
        {
            //if (!marker.enabled) marker.enabled = true;
            Vector3 screenPos = Camera.main.WorldToScreenPoint(selected.transform.position);
            // add a tiny bit of height?
            screenPos.y += 5f; // adjust as you see fit.
            //marker.transform.position = screenPos;
        }
    }

    public void OnSelect(Selectable selected)
    {
        if(this.selected != null && selected != this.selected)
        {
            this.selected.OnDeselect();
            //marker.enabled = false;
        }

        this.selected = selected;
    }
}
