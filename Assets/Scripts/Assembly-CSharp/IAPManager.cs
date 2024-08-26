using System;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Security;

public class IAPManager : MonoBehaviour, IStoreListener
{
	public static IAPManager cs = null;

	private static IStoreController m_StoreController;

	private static IExtensionProvider m_StoreExtensionProvider;

	public static string kProductIDConsumable = "consumable";

	public static string kProductIDNonConsumable = "nonconsumable";

	public static string kProductIDSubscription = "subscription";

	public static string item_0 = "coinpack_0";

	public static string item_1 = "coinpack_1";

	public static string item_2 = "coinpack_2";

	public static string item_3 = "coinpack_3";

	public static string item_4 = "coinpack_4";

	public static string item_5 = "coinpack_5";

	private static string kProductNameAppleSubscription = "com.unity3d.subscription.new";

	private static string kProductNameGooglePlaySubscription = "com.unity3d.subscription.original";

	private void Start()
	{
		cs = this;
		if (m_StoreController == null)
		{
			InitializePurchasing();
		}
	}

	public void InitializePurchasing()
	{
		if (!IsInitialized())
		{
			ConfigurationBuilder configurationBuilder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
			configurationBuilder.AddProduct(item_1, ProductType.Consumable);
			configurationBuilder.AddProduct(item_2, ProductType.Consumable);
			configurationBuilder.AddProduct(item_3, ProductType.Consumable);
			configurationBuilder.AddProduct(item_4, ProductType.Consumable);
			configurationBuilder.AddProduct(item_5, ProductType.Consumable);
			UnityPurchasing.Initialize(this, configurationBuilder);
			Debug.Log("InitializePurchasing()");
		}
	}

	private bool IsInitialized()
	{
		if (m_StoreController != null)
		{
			return m_StoreExtensionProvider != null;
		}
		return false;
	}

	public void BuyConsumable()
	{
		BuyProductID(kProductIDConsumable);
	}

	public void BuyItem(int id)
	{
		if (id >= 0 && id <= 5)
		{
			BuyProductID("coinpack_" + id);
			Debug.Log("BuyItem " + id);
		}
	}

	public void BuyNonConsumable()
	{
		BuyProductID(kProductIDNonConsumable);
	}

	public void BuySubscription()
	{
		BuyProductID(kProductIDSubscription);
	}

	private void BuyProductID(string productId)
	{
		if (IsInitialized())
		{
			Product product = m_StoreController.products.WithID(productId);
			if (product != null && product.availableToPurchase)
			{
				Debug.Log(string.Format("Purchasing product asychronously: '{0}'", product.definition.id));
				m_StoreController.InitiatePurchase(product);
			}
			else
			{
				Debug.Log("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase (" + productId + ")");
			}
		}
		else
		{
			Debug.Log("BuyProductID FAIL. Not initialized.");
		}
	}

	public void RestorePurchases()
	{
		if (!IsInitialized())
		{
			Debug.Log("RestorePurchases FAIL. Not initialized.");
		}
		else if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.OSXPlayer)
		{
			Debug.Log("RestorePurchases started ...");
			m_StoreExtensionProvider.GetExtension<IAppleExtensions>().RestoreTransactions(delegate(bool result)
			{
				Debug.Log("RestorePurchases continuing: " + result + ". If no further messages, no purchases available to restore.");
			});
		}
		else
		{
			Debug.Log("RestorePurchases FAIL. Not supported on this platform. Current = " + Application.platform);
		}
	}

	public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
	{
		Debug.Log("OnInitialized: PASS");
		m_StoreController = controller;
		m_StoreExtensionProvider = extensions;
		string localizedPriceString = m_StoreController.products.WithID("coinpack_1").metadata.localizedPriceString;
		string localizedPriceString2 = m_StoreController.products.WithID("coinpack_2").metadata.localizedPriceString;
		string localizedPriceString3 = m_StoreController.products.WithID("coinpack_3").metadata.localizedPriceString;
		string localizedPriceString4 = m_StoreController.products.WithID("coinpack_4").metadata.localizedPriceString;
		string localizedPriceString5 = m_StoreController.products.WithID("coinpack_5").metadata.localizedPriceString;
		GUIGold.cs.UpdatePrice("coinpack_1", localizedPriceString);
		GUIGold.cs.UpdatePrice("coinpack_2", localizedPriceString2);
		GUIGold.cs.UpdatePrice("coinpack_3", localizedPriceString3);
		GUIGold.cs.UpdatePrice("coinpack_4", localizedPriceString4);
		GUIGold.cs.UpdatePrice("coinpack_5", localizedPriceString5);
	}

	public void OnInitializeFailed(InitializationFailureReason error)
	{
		Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
	}

	public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
	{
		string id = args.purchasedProduct.definition.id;
		int num = -1;
		if (string.Equals(id, "coinpack_1", StringComparison.Ordinal))
		{
			num = 1;
		}
		else if (string.Equals(id, "coinpack_2", StringComparison.Ordinal))
		{
			num = 2;
		}
		else if (string.Equals(id, "coinpack_3", StringComparison.Ordinal))
		{
			num = 3;
		}
		else if (string.Equals(id, "coinpack_4", StringComparison.Ordinal))
		{
			num = 4;
		}
		else if (string.Equals(id, "coinpack_5", StringComparison.Ordinal))
		{
			num = 5;
		}
		Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}' > itemid {0}", args.purchasedProduct.definition.id, num));
		Log.AddMainLog(string.Format("ProcessPurchase: PASS. Product: '{0}' > itemid {0}", args.purchasedProduct.definition.id, num));
		Debug.Log("Transaction: " + args.purchasedProduct.transactionID);
		Log.AddMainLog("Transaction: " + args.purchasedProduct.transactionID);
		string text = "null";
		bool flag = true;
		try
		{
			IPurchaseReceipt[] array = new CrossPlatformValidator(GooglePlayTangle.Data(), null, Application.identifier).Validate(args.purchasedProduct.receipt);
			Log.AddMainLog("Receipt is valid. Contents:");
			IPurchaseReceipt[] array2 = array;
			foreach (IPurchaseReceipt obj in array2)
			{
				Debug.Log(obj.productID);
				Debug.Log(obj.purchaseDate);
				Debug.Log(obj.transactionID);
				GooglePlayReceipt googlePlayReceipt = obj as GooglePlayReceipt;
				if (googlePlayReceipt != null)
				{
					text = googlePlayReceipt.purchaseToken;
				}
			}
		}
		catch (IAPSecurityException)
		{
			Log.AddMainLog("Invalid receipt");
			flag = false;
		}
		Log.AddMainLog("Itemdefid: " + id + " Token: " + text);
		if (flag)
		{
			int storeReceipt = GetStoreReceipt();
			if (storeReceipt >= 0)
			{
				SetStoreReceipt(storeReceipt, Main.gid, id, text);
			}
			Log.AddMainLog("R: " + id + " " + text);
			MasterClient.cs.send_purchasegoogle(id, text);
		}
		return PurchaseProcessingResult.Complete;
	}

	public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
	{
		Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));
	}

	public int GetStoreReceipt()
	{
		for (int i = 0; i < 16; i++)
		{
			if (!PlayerPrefs.HasKey("bp_sr" + i + "_" + Main.gid))
			{
				return i;
			}
		}
		return -1;
	}

	public void SetStoreReceipt(int id, int gid, string s0, string s1)
	{
		PlayerPrefs.SetString("bp_sr" + id + "_" + gid, s0);
		PlayerPrefs.SetString("bp_sr" + id + "_" + gid + "_", s1);
	}

	public void DelStoreReceipt(int gid, string s0, string s1)
	{
		for (int i = 0; i < 16; i++)
		{
			if (PlayerPrefs.HasKey("bp_sr" + i + "_" + gid))
			{
				string @string = PlayerPrefs.GetString("bp_sr" + i + "_" + gid);
				string string2 = PlayerPrefs.GetString("bp_sr" + i + "_" + gid + "_");
				if (@string == s0 && string2 == s1)
				{
					PlayerPrefs.DeleteKey("bp_sr" + i + "_" + gid);
					PlayerPrefs.DeleteKey("bp_sr" + i + "_" + gid + "_");
					break;
				}
			}
		}
	}

	public void SyncIAP()
	{
		if (Main.gid == 0)
		{
			return;
		}
		for (int i = 0; i < 16; i++)
		{
			if (PlayerPrefs.HasKey("bp_sr" + i + "_" + Main.gid))
			{
				PlayerPrefs.GetString("bp_sr" + i + "_" + Main.gid);
				PlayerPrefs.GetString("bp_sr" + i + "_" + Main.gid + "_");
				break;
			}
		}
	}
}
