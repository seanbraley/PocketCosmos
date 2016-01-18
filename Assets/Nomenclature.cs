using UnityEngine;
using System.Collections;

public class Nomenclature {

	private static string[] _consonants = new string[]
	{
		"b","c","ch","d","f","g","h","j","k","l","m","n","p","pr","q","r","s","t","v","w","x","z"
	};

	private static string[] _vowels = new string[]
	{
		"a","e","i","o","u","y"
	};

	public static string GetRandomWord() {
		string word = "";
		for (int i = 0; i < 3; i++) {
			word += GetRandomWordChunk();
		}
		return word;
	}

	private static string GetRandomWordChunk() {
		string chunk = "";
		if (Random.value < 0.75) {
			chunk += _consonants[Random.Range(0,_consonants.Length)];
		}

		chunk += _vowels[Random.Range(0,_vowels.Length)];

		if (Random.value < 0.25) {
			chunk += _vowels[Random.Range(0,_vowels.Length)];
		}

		return chunk;
	}
}
