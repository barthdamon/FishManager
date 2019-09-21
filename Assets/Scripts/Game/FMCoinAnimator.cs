using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FMCoinAnimator : MonoBehaviour
{
	[Tooltip("Curve to control the rate of show/hide for coins spawned.")]
	public AnimationCurve m_FadeCurve;

	[Tooltip("Curve to control the rate of movement in the Y axis for the coins spawned.")]
	public AnimationCurve m_MovementCurve;

	public float m_MaxLateralMovement = 5f;

	public Vector3 m_offsetPosition = Vector3.zero;
	public Vector3 m_offsetUpMax = Vector3.up * 5f;

	private UnityEngine.UI.Image m_coin_image;
	private Color m_start_colour;
	private Vector3 m_start_position;
	private float m_x_axis_movement_scalar = 0f;
	private float m_life_time = 0f;

    void Awake()
    {
		m_coin_image = GetComponent<UnityEngine.UI.Image>();
		m_start_colour = m_coin_image.color;
		m_start_position = m_coin_image.transform.position;
		m_x_axis_movement_scalar = Random.Range(-m_MaxLateralMovement, m_MaxLateralMovement);
		m_offsetUpMax += m_start_position;
		m_offsetUpMax.x += m_x_axis_movement_scalar;
	}

    void Update()
    {
		m_life_time += Time.deltaTime / 2f;
		m_coin_image.color = Color.Lerp(m_start_colour, m_start_colour.Adjusted(a:0f), m_FadeCurve.Evaluate(m_life_time));
		this.transform.position = Vector3.Lerp(m_start_position, m_offsetUpMax, m_MovementCurve.Evaluate(m_life_time / 2f));

		if (m_life_time >= 1f)
		{
			this.enabled = false;
			this.transform.parent = null;
			Destroy(this.gameObject);
		}
    }
}
