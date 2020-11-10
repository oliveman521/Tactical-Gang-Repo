using UnityEngine;
using UnityEngine.EventSystems;

public class EventTriggerHandler : MonoBehaviour
{
	public void OnPointerEnter(BaseEventData eventData)
	{
		Debug.LogFormat("OnPointerEnter - eventData: {0}", (PointerEventData)eventData);
	}

	public void OnPointerExit(BaseEventData eventData)
	{
		Debug.LogFormat("OnPointerExit - eventData: {0}", (PointerEventData)eventData);
	}

	public void OnPointerDown(BaseEventData eventData)
	{
		Debug.LogFormat("OnPointerDown - eventData: {0}", (PointerEventData)eventData);
	}

	public void OnPointerUp(BaseEventData eventData)
	{
		Debug.LogFormat("OnPointerDown - eventData: {0}", (PointerEventData)eventData);
	}

	public void OnPointerClick(BaseEventData eventData)
	{
		Debug.LogFormat("OnPointerClick - eventData: {0}", (PointerEventData)eventData);
	}

	public void OnDrag(BaseEventData eventData)
	{
		Debug.LogFormat("OnDrag - eventData: {0}", (PointerEventData)eventData);
	}

	public void OnDrop(BaseEventData eventData)
	{
		Debug.LogFormat("OnDrop - eventData: {0}", (PointerEventData)eventData);
	}

	public void OnScroll(BaseEventData eventData)
	{
		Debug.LogFormat("OnScroll - eventData: {0}", (PointerEventData)eventData);
	}

	public void OnUpdateSelected(BaseEventData eventData)
	{
		Debug.LogFormat("OnUpdateSelected - eventData: {0}", eventData);
	}

	public void OnSelect(BaseEventData eventData)
	{
		Debug.LogFormat("OnSelect - eventData: {0}", eventData);
	}

	public void OnDeselect(BaseEventData eventData)
	{
		Debug.LogFormat("OnDeselect - eventData: {0}", eventData);
	}

	public void OnMove(BaseEventData eventData)
	{
		Debug.LogFormat("OnMove - eventData: {0}", (AxisEventData)eventData);
	}

	public void OnInitializePotentialDrag(BaseEventData eventData)
	{
		Debug.LogFormat("OnInitializePotentialDrag - eventData: {0}", (PointerEventData)eventData);
	}

	public void OnBeginDrag(BaseEventData eventData)
	{
		Debug.LogFormat("OnBeginDrag - eventData: {0}", (PointerEventData)eventData);
	}

	public void OnEndDrag(BaseEventData eventData)
	{
		Debug.LogFormat("OnEndDrag - eventData: {0}", eventData);
	}

	public void OnSubmit(BaseEventData eventData)
	{
		Debug.LogFormat("OnSubmit - eventData: {0}", eventData);
	}

	public void OnCancel(BaseEventData eventData)
	{
		Debug.LogFormat("OnCancel - eventData: {0}", eventData);
	}
}