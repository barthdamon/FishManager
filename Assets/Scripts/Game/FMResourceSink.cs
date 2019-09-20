using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FMResourceSink : FMTaskBase
{
	// demand
	// STANDARD resource consumption rate across all sinks
	public static float m_ConsumerTickRate = 1f;
	public static float m_ConsumerTickResourceAmount = 1f;

	[Tooltip("High value at 0 scaling down to close to zero at infinity")]
	public AnimationCurve m_DemandValueCurve;

	public float m_CurrentSicknessLevel = 0f;

	// Demand is represented by a crowd, but determined by resource amount

	// resource amount - determines your capital
	// replenished by the processor

	// outputs capital
	// outputs sickness (based on food quality) which affects worker productivity

	private void Start()
	{
		FMGameLoopManager.GetOrCreateInstance().m_OnDayStartEvent += OnDayStart;
	}

	public void OnDayStart(FMDay day)
	{
		m_CurrentSicknessLevel = 0f;
	}

	protected override void TriggerTask()
	{
		base.TriggerTask();

		// determine capital gain
		float totalAmount = m_Resources.Sum((FMResource res) => res.m_Amount);
		float capitalGain = m_DemandValueCurve.Evaluate(totalAmount);
		FMPlayer.GetOrCreateInstance().m_Capital += capitalGain;

		// deplete resource
		var resourcesToDeplete = m_ConsumerTickResourceAmount;
		float triggeredSickness = 0f;
		var depletedAmount = 0f;
		while (depletedAmount < m_ConsumerTickResourceAmount && m_Resources.Count > 0)
		{
			var resource = m_Resources.Peek();
			var thisDepletion = Mathf.Min(resource.m_ProcessedAmount, resourcesToDeplete);

			float percentageSicknessToUse = thisDepletion / m_ConsumerTickResourceAmount;
			resource.m_ProcessedAmount -= thisDepletion;
			triggeredSickness += percentageSicknessToUse * resource.m_Sickness;
			depletedAmount += thisDepletion;
			resourcesToDeplete -= thisDepletion;

			if (resource.m_ProcessedAmount <= 0)
				m_Resources.Dequeue();
		}

		// get people sick
		m_CurrentSicknessLevel += triggeredSickness;
	}

	public override float GetTimeToTrigger()
	{
		return m_ConsumerTickRate;
	}
}
