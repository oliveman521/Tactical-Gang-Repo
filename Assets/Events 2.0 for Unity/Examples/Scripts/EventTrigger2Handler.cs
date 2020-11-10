using UnityEngine;
using UnityEngine.EventSystems;

public class EventTrigger2Handler : MonoBehaviour
{
	public void OnPointerEnter(BaseEventData eventData, string s)
	{
		Debug.LogFormat("OnPointerEnter - string: {1}, eventData: {0}", (PointerEventData)eventData, s);
	}

	public void OnPointerExit(BaseEventData eventData, string s)
	{
		Debug.LogFormat("OnPointerExit - string: {1}, eventData: {0}", (PointerEventData)eventData, s);
	}

	public void OnPointerDown(BaseEventData eventData, string s)
	{
		Debug.LogFormat("OnPointerDown - string: {1}, eventData: {0}", (PointerEventData)eventData, s);
	}

	public void OnPointerUp(BaseEventData eventData, string s)
	{
		Debug.LogFormat("OnPointerDown - string: {1}, eventData: {0}", (PointerEventData)eventData, s);
	}

	public void OnPointerClick(BaseEventData eventData, string s)
	{
		Debug.LogFormat("OnPointerClick - string: {1}, eventData: {0}", (PointerEventData)eventData, s);
	}

	public void OnDrag(BaseEventData eventData, string s)
	{
		Debug.LogFormat("OnDrag - string: {1}, eventData: {0}", (PointerEventData)eventData, s);
	}

	public void OnDrop(BaseEventData eventData, string s)
	{
		Debug.LogFormat("OnDrop - string: {1}, eventData: {0}", (PointerEventData)eventData, s);
	}

	public void OnScroll(BaseEventData eventData, string s)
	{
		Debug.LogFormat("OnScroll - string: {1}, eventData: {0}", (PointerEventData)eventData, s);
	}

	public void OnUpdateSelected(BaseEventData eventData, string s)
	{
		Debug.LogFormat("OnUpdateSelected - string: {1}, eventData: {0}", eventData, s);
	}

	public void OnSelect(BaseEventData eventData, string s)
	{
		Debug.LogFormat("OnSelect - string: {1}, eventData: {0}", eventData, s);
	}

	public void OnDeselect(BaseEventData eventData, string s)
	{
		Debug.LogFormat("OnDeselect - string: {1}, eventData: {0}", eventData, s);
	}

	public void OnMove(BaseEventData eventData, string s)
	{
		Debug.LogFormat("OnMove - string: {1}, eventData: {0}", (AxisEventData)eventData, s);
	}

	public void OnInitializePotentialDrag(BaseEventData eventData, string s)
	{
		Debug.LogFormat("OnInitializePotentialDrag - string: {1}, eventData: {0}", (PointerEventData)eventData, s);
	}

	public void OnBeginDrag(BaseEventData eventData, string s)
	{
		Debug.LogFormat("OnBeginDrag - string: {1}, eventData: {0}", (PointerEventData)eventData, s);
	}

	public void OnEndDrag(BaseEventData eventData, string s)
	{
		Debug.LogFormat("OnEndDrag - string: {1}, eventData: {0}", eventData, s);
	}

	public void OnSubmit(BaseEventData eventData, string s)
	{
		Debug.LogFormat("OnSubmit - string: {1}, eventData: {0}", eventData, s);
	}

	public void OnCancel(BaseEventData eventData, string s)
	{
		Debug.LogFormat("OnCancel - string: {1}, eventData: {0}", eventData, s);
	}
}