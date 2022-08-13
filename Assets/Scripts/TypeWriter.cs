using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TypeWriter : MonoBehaviour
{
	private Text text;
	private TMP_Text tmpProText;
	private string writer;
	private const string LeadingChar = "█";

	[SerializeField] private float delayBeforeStart = 0f;
	[SerializeField] private float timeBtwChars = 0.1f;
	[SerializeField] private bool leadingCharBeforeDelay = false;

	// Use this for initialization
	private void Start()
	{
		text = GetComponent<Text>()!;
		tmpProText = GetComponent<TMP_Text>()!;

		if (text != null)
		{
			writer = text.text;
			text.text = "";

			StartCoroutine(TypeWriterText());
		}

		if (tmpProText != null)
		{
			writer = tmpProText.text;
			tmpProText.text = "";

			StartCoroutine(TypeWriterTMP());
		}
	}

	private IEnumerator TypeWriterText()
	{
		text.text = leadingCharBeforeDelay ? LeadingChar : "";

		yield return new WaitForSeconds(GetRandomDelayBeforeStart(delayBeforeStart));

		foreach (char c in writer)
		{
			if (text.text.Length > 0)
			{
				text.text = text.text.Substring(0, text.text.Length - LeadingChar.Length);
			}

			text.text += c;
			text.text += LeadingChar;
			yield return new WaitForSeconds(timeBtwChars);
		}

		if (LeadingChar != "")
		{
			text.text = text.text.Substring(0, text.text.Length - LeadingChar.Length);
		}
	}

	private IEnumerator TypeWriterTMP()
	{
		tmpProText.text = leadingCharBeforeDelay ? LeadingChar : "";

		yield return new WaitForSeconds(GetRandomDelayBeforeStart(delayBeforeStart));

		foreach (char c in writer)
		{
			if (tmpProText.text.Length > 0)
			{
				tmpProText.text = tmpProText.text.Substring(0, tmpProText.text.Length - LeadingChar.Length);
			}

			tmpProText.text += c;
			tmpProText.text += LeadingChar;
			yield return new WaitForSeconds(timeBtwChars);
		}

		if (LeadingChar != "")
		{
			tmpProText.text = tmpProText.text.Substring(0, tmpProText.text.Length - LeadingChar.Length);
		}
	}

	private float GetRandomDelayBeforeStart(float delay)
	{
		delay *= Random.Range(0.8f, 1.2f);
		return delay;
	} 
}