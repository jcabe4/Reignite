/************************************************************
 * Script: XMLFileManager.cs
 * Author: Jonathan Robinson
 * Date:2.12.2014
 * Description: Both Encrypt and Decrypt Algorythms are
 * taken and slightly modified from
 * http://unitynoobs.blogspot.com/2012/01/xml-encrypting.html
 * Based on work from Jonathan Cabe
 ************************************************************/

using UnityEngine;
using System;
using System.IO;
using System.Xml;
using System.Text;
using System.Security.Cryptography;

[ExecuteInEditMode]
public class XMLFileManager : MonoBehaviour 
{
	public static void EncryptFile (string path)
	{
		if (!File.Exists(path))
		{
			Debug.Log("File Not Found At: " + path);
			return;
		}
	
		XmlDocument xmlFile = new XmlDocument();
		xmlFile.Load(path);
		XmlElement xmlRoot = xmlFile.DocumentElement;
		
		if (xmlRoot.ChildNodes.Count > 1)
		{
			byte[] keyArray = UTF8Encoding.UTF8.GetBytes ("86759426197648123460789546213421");
			byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes (xmlRoot.InnerXml);
			RijndaelManaged rDel = new RijndaelManaged();
			rDel.Key = keyArray;
			rDel.Mode = CipherMode.ECB;
			rDel.Padding = PaddingMode.PKCS7;
			ICryptoTransform cTransform = rDel.CreateEncryptor();
			byte[] resultArray = cTransform.TransformFinalBlock (toEncryptArray, 0, toEncryptArray.Length);
			xmlRoot.InnerXml = Convert.ToBase64String (resultArray, 0, resultArray.Length);
		}
		
		xmlFile.Save(path);
	}
	
	public static void DecryptFile (string path)
	{
		if (!File.Exists(path))
		{
			Debug.Log("File Not Found At: " + path);
			return;
		}
	
		XmlDocument xmlFile = new XmlDocument();
		xmlFile.Load(path);
		XmlElement xmlRoot = xmlFile.DocumentElement;
		
		if (xmlRoot.ChildNodes.Count <= 1)
		{
			byte[] keyArray = UTF8Encoding.UTF8.GetBytes ("86759426197648123460789546213421");
			byte[] toEncryptArray = Convert.FromBase64String (xmlRoot.InnerXml);
			RijndaelManaged rDel = new RijndaelManaged();
			rDel.Key = keyArray;
			rDel.Mode = CipherMode.ECB;
			rDel.Padding = PaddingMode.PKCS7;
			ICryptoTransform cTransform = rDel.CreateDecryptor();
			byte[] resultArray = cTransform.TransformFinalBlock (toEncryptArray, 0, toEncryptArray.Length);
			xmlRoot.InnerXml = UTF8Encoding.UTF8.GetString (resultArray);
		}
	}
}
