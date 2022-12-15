using UnityEngine;
using Cinemachine;

[CreateAssetMenu]
public class CinemachineImpulseData : ScriptableObject
{

	static readonly Vector3 k_DefaultVelocity = Vector3.down;

	[SerializeField, CinemachineImpulseDefinitionProperty]
	CinemachineImpulseDefinition m_Definition = new CinemachineImpulseDefinition();

	public CinemachineImpulseDefinition Definition => m_Definition;

	public void GenerateImpulse(Vector3 position)
	{
		m_Definition.CreateEvent(position, k_DefaultVelocity);
	}

	public void GenerateImpulse(Vector3 position, Vector3 velocity)
	{
		m_Definition.CreateEvent(position, velocity);
	}

#if UNITY_EDITOR
	void OnValidate()
	{
		m_Definition.OnValidate();
	}
#endif

}