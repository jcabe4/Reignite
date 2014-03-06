using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Conversation 
{
	public bool displayed = false;
	public Quote newQuote = new Quote();
	public List<Quote> quotes = new List<Quote>();
	
	public void SortQuotes()
	{
		int min;
		Quote temp;
		
		for (int i = 0; i < quotes.Count; i++)
		{
			min = i;
			
			for (int j = i; j < quotes.Count; j++)
			{
				if (quotes[min].quoteIndex > quotes[j].quoteIndex)
				{
					min = j;
				}
			}
			
			temp = quotes[min];
			quotes[min] = quotes[i];
			quotes[i] = temp;
		}
		
		for (int i = 0; i < quotes.Count; i++)
		{
			quotes[i].quoteIndex = i;
		}
	}
	
	public bool AddQuote(Quote quote)
	{
		bool bCanAdd = true;
		
		for (int i = 0; i < quotes.Count; i++)
		{
			if (quote.quoteIndex == quotes[i].quoteIndex)
			{
				bCanAdd = false;
			}
		}
		
		if (bCanAdd)
		{
			quotes.Add(quote);
			SortQuotes();
			
			return true;
		}
		else
		{
			return false;
		}
	}
}
