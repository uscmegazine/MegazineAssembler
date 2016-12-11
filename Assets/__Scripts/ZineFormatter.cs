using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class ZineFormatter : MonoBehaviour
{
	public enum PageType
	{
		A,
		B
	}

	[SerializeField] protected RectTransform _pageParent;
	[SerializeField] protected Image _topLeftImage;
	[SerializeField] protected Image _topRightImage;
	[SerializeField] protected Image _bottomLeftImage;
	[SerializeField] protected Image _bottomRightImage;
	[SerializeField] protected PageCollection _pageCollection;

	protected bool _formattingZine;
	protected PageType _currPageType;
	protected int _currPageIndex;
	protected float _lastXPosition;
	protected float _padding = 100;
	protected int _outputPageIndex;

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			FormatZine();
		}
	}

	public void FormatZine()
	{
		if (!_formattingZine)
		{
			StartCoroutine(DoFormatZine());
		}
		else
		{
			Debug.LogError("Can't format zine while it's already being formatted!");
		}
	}

	protected IEnumerator DoFormatZine()
	{
		_formattingZine = true;
		_currPageType = PageType.A;
		_currPageIndex = 0;
		_outputPageIndex = 0;
		_lastXPosition = _pageParent.anchoredPosition.x;

		for (int j = 0; j < _pageCollection.pages.Count; j += 8)
		{
			_currPageIndex = j;
			Sprite[] currPages = new Sprite[8];

			for (int i = _currPageIndex; i < _currPageIndex + 8; i++)
			{
				if (i < _pageCollection.pages.Count)
				{
					currPages[i - _currPageIndex] = _pageCollection.pages[i];
				}
				else
				{
					currPages[i - _currPageIndex] = null;
				}
			}


			// A side
			_currPageType = PageType.A;
			PlacePage(currPages[2], _topLeftImage, true);
			PlacePage(currPages[1], _topRightImage, true);
			PlacePage(currPages[5], _bottomLeftImage, false);
			PlacePage(currPages[6], _bottomRightImage, false);
			TakePicture();
			yield return new WaitForSeconds(0.2f);

			// B side
			_currPageType = PageType.B;
			PlacePage(currPages[0], _topLeftImage, true);
			PlacePage(currPages[3], _topRightImage, true);
			PlacePage(currPages[7], _bottomLeftImage, false);
			PlacePage(currPages[4], _bottomRightImage, false);
			TakePicture();
			_outputPageIndex++;
			yield return new WaitForSeconds(0.2f);

		}

		//UnityEditor.AssetDatabase.Refresh();
		_formattingZine = false;
	}

	protected void PlacePage(Sprite page, Image image, bool upsideDown)
	{
		image.sprite = page;

		if (upsideDown)
		{
			image.transform.localEulerAngles = new Vector3(0, 0, 180);
		}
		else
		{
			image.transform.localEulerAngles = new Vector3(0, 0, 0);
		}
	}

	protected void TakePicture()
	{
		string path = Path.Combine("Output", string.Format("Vol{0}_Page{1}{2}.png", _pageCollection.volume, _outputPageIndex, _currPageType));
		Application.CaptureScreenshot(path);
		RectTransform copy = Instantiate(_pageParent, _pageParent.parent) as RectTransform;
		copy.anchoredPosition = new Vector2(_lastXPosition + copy.rect.width + _padding, copy.anchoredPosition.y);
		_lastXPosition += (copy.rect.width + _padding);
	}
}
