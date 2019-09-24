using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FMCapitalDisplay : MonoBehaviour
{
    [Tooltip("Curve to control the rate of show/hide for coins spawned.")]
    public AnimationCurve m_FadeCurve;

    [Tooltip("Curve to control the rate of movement in the Y axis for the coins spawned.")]
    public AnimationCurve m_MovementCurve;

    public Color m_GoodColour = Color.green;

    public Color m_BadColour = Color.red;

    public GameObject m_DeltaPrefab;

    public Text m_CapitalDisplayText;

    public float multiplier = 1000;

    private void Awake()
    {
        FMPlayer.GetOrCreateInstance().OnCapitalIncrease += OnCapitalIncrease;
    }

    private void OnCapitalIncrease(float value)
    {
        StartCoroutine(CreateCapitalTickDisplay(value));
    }

    // Update is called once per frame
    void Update()
    {
        float value = FMPlayer.GetOrCreateInstance().m_Capital;

        value *= multiplier;  //because humans' psychology ;)

        m_CapitalDisplayText.text = value.ToString();
    }

    private IEnumerator CreateCapitalTickDisplay(float value)
    {
        value *= multiplier;  //because humans' psychology ;)

        // Create
        yield return new WaitForSeconds(Random.Range(0f, 0.75f));

        var game_object = Instantiate(m_DeltaPrefab, this.transform.position, Quaternion.identity, this.transform);
        var text = game_object.GetComponent<Text>();
        text.text = value > 0f ? "+" + value : value.ToString();
        var transform = game_object.transform;
        var start_position = transform.position;
        start_position.x += Random.Range(-150f, 150f);
        var start_colour = value > 0 ? m_GoodColour : m_BadColour;
        Vector3 end_pos = start_position + Vector3.up * Random.Range(10f, 25f);

        float t = 0f;
        while (t < 1f)
        {
            // Animate
            t += Time.deltaTime / 2f;
            text.color = Color.Lerp(
                start_colour,
                start_colour.Adjusted(a: 0f),
                m_FadeCurve.Evaluate(t));
            transform.position = Vector3.Lerp(
                start_position,
                end_pos,
                m_MovementCurve.Evaluate(t));
            yield return null;
        }

        game_object.SetActive(false);
        Destroy(game_object);
    }
}
