using System.Collections.Generic;
using Ketsu.Utils;
using UnityEngine;

public class Orbit : MonoBehaviour
{
	public Vector3 OrbitAxis = Vector3.up;
	public Vector3 CrossAxis = Vector3.forward;
	public float OrbitAltitude = 2f;
	public float OrbitDegreesPerSec = 180.0f;
	public GameObject OrbiterPrefab;

	private Quaternion _fixedRotation;
	private List<Orbiter> _orbiters;

	private void Awake()
	{
		_orbiters = new List<Orbiter>();
		_fixedRotation = transform.rotation;
	}

	public void Clear()
	{
		Remove(_orbiters.Count);
	}

	public void AddOrRemove(int finalAmount)
	{
		var diff = finalAmount - _orbiters.Count;
		if (diff > 0) Add(diff);
		else if (diff < 0) Remove(Mathf.Abs(diff));
	}
	
	public void AddOrConsume(int finalAmount)
	{
		var diff = finalAmount - _orbiters.Count;
		if (diff > 0) Add(diff);
		else if (diff < 0) Consume(Mathf.Abs(diff));
	}

	public void Add(int amount)
	{
		for (var i = 0; i < amount; ++i)
		{
			var orbiter = Instantiate(OrbiterPrefab, transform).GetComponent<Orbiter>();
			orbiter.transform.position = transform.position + CrossAxis * OrbitAltitude;
			orbiter.RelativePosition = orbiter.transform.position - transform.position;
			_orbiters.Add(orbiter);
		}
		UpdateRelativePositions();
	}

	public void Remove(int amount)
	{
		for (var i = amount - 1; i >= 0; --i)
		{
			var orbiter = _orbiters[i];
			_orbiters.RemoveAt(i);
			orbiter.Destroy();
		}
	}

	public void Consume(int amount)
	{
		var count = Mathf.Min(amount, _orbiters.Count);
		for (var i = count - 1; i >= 0; --i)
		{
			var orbiter = _orbiters[i];
			_orbiters.RemoveAt(i);
			orbiter.Consume();
		}
		
		UpdateRelativePositions();
	}

	private void UpdateRelativePositions()
	{
		for (var i = 1; i < _orbiters.Count; ++i)
		{
			var orbiter = _orbiters[i];
			orbiter.transform.position = _orbiters[0].transform.position;
			orbiter.transform.RotateAround(transform.position, OrbitAxis, 360f / _orbiters.Count * i);
			orbiter.RelativePosition = orbiter.transform.position - transform.position;
		}
	}

	private void LateUpdate()
	{
		transform.rotation = _fixedRotation;
		foreach (var orbiter in _orbiters)
		{
			orbiter.transform.position = transform.position + orbiter.RelativePosition;
			orbiter.transform.RotateAround(transform.position, OrbitAxis, OrbitDegreesPerSec * Time.deltaTime);
			orbiter.RelativePosition = orbiter.transform.position - transform.position;
		}
	}
}