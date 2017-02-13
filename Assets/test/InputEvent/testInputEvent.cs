using UnityEngine;
using System.Collections;

public class testInputEvent : MonoBehaviour {
	void Start () {
        MySkyInputEvent inputEvent = gameObject.AddComponent<MySkyInputEvent>();
        MySkyInputEvent.instance.SetCamera(this.gameObject.GetComponent<Camera>());
        MySkyInputEvent.instance.SetUICamera(this.gameObject.GetComponent<Camera>());

        Messenger.AddListener<GameObject, MySkyInputEvent.DragState, Vector2, Vector2, Vector3>(MySkyInputEvent.EventDrag, EventDrag);
	    
    }
	
	// Update is called once per frame
	void Update () {
	    
	}
    public string outString = "";
    public float allX = 0;
    public void EventDrag(GameObject _gameObject, MySkyInputEvent.DragState _state, 
        Vector2 _screenPos, Vector2 _deltaPos, Vector3 _objPos)
    {
        outString = "_gameObject:" + _gameObject +
            " \n_state:" + _state +
            " \n_screenPos:" + _screenPos +
            " \n_deltaPos:" + _deltaPos +
            " \n_objPos:" + _objPos;
        Debug.Log(outString);
        if (_gameObject != null && _state == MySkyInputEvent.DragState.OnDrag)
            _gameObject.transform.position = _objPos;
        if(_state == MySkyInputEvent.DragState.Start)
        {
            allX = 0;
        }
        if(_state == MySkyInputEvent.DragState.OnDrag)
        {
            allX += _deltaPos.x;
            outString += allX;
        }
    }

    void OnGUI()
    {
        GUI.Label(new Rect(0, 0, 1000, 1000), outString);
    }
}
