﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FMResourceSink : FMTaskBase
{
	public static float m_ConsumerTickRate = 1f;
	public static float m_ConsumerTriggerResourceAmount = 1f;

	[Tooltip("High value at 0 scaling down to close to zero at infinity")]
	public AnimationCurve m_DemandValueCurve;

	public float m_CurrentSicknessLevel = 0f;

	public override bool AcceptsWorkers()
	{
		return false;
	}

	// sinks don't take on workers
	public override bool AssignWorker(FMWorker worker)
	{
		return false;
	}

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
		float totalAmount = m_Resources.Sum((FMResource res) => res.m_ProcessedAmount);
		float capitalGain = m_DemandValueCurve.Evaluate(totalAmount);
		FMPlayer.GetOrCreateInstance().m_Capital += capitalGain;

		// deplete resource
		var resourcesToDeplete = m_ConsumerTriggerResourceAmount;
		float triggeredSickness = 0f;
		var depletedAmount = 0f;
		while (depletedAmount < m_ConsumerTriggerResourceAmount && m_Resources.Count > 0)
		{
			var resource = m_Resources.Peek();
			var thisDepletion = Mathf.Min(resource.m_ProcessedAmount, resourcesToDeplete);

			float percentageSicknessToUse = thisDepletion / m_ConsumerTriggerResourceAmount;
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

	protected override void ShutDown()
	{
		// PEOPLE ALWAYS EAT!
	}
}
