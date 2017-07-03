using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.SceneManagement;

public class Purchaser : MonoBehaviour, IStoreListener
{
	private static IStoreController m_StoreController;
	private static IExtensionProvider m_StoreExtensionProvider;

	public PlayScreen playScreen;

	public static string GEMS_5 = "gems_5";
	public static string GEMS_30 = "gems_30";
	public static string GEMS_70 = "gems_70";
	public static string GEMS_150 = "gems_150";
	public static string GEMS_999 = "gems_999";
	public static string AD_OFF = "ad_off";

	void Start()
	{
		if (m_StoreController == null)
		{
			InitializePurchasing();
		}
	}

	public void InitializePurchasing() 
	{
		if (IsInitialized())
		{
			return;
		}


		var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

		builder.AddProduct(GEMS_5, ProductType.Consumable, new IDs(){{ GEMS_5, AppleAppStore.Name}, { GEMS_5, GooglePlay.Name} });
		builder.AddProduct(GEMS_30, ProductType.Consumable, new IDs(){{ GEMS_30, AppleAppStore.Name}, { GEMS_30, GooglePlay.Name} });
		builder.AddProduct(GEMS_70, ProductType.Consumable, new IDs(){{ GEMS_70, AppleAppStore.Name}, { GEMS_70, GooglePlay.Name} });
		builder.AddProduct(GEMS_150, ProductType.Consumable, new IDs(){{ GEMS_150, AppleAppStore.Name}, { GEMS_150, GooglePlay.Name} });
		builder.AddProduct(GEMS_999, ProductType.Consumable, new IDs(){{ GEMS_999, AppleAppStore.Name}, { GEMS_999, GooglePlay.Name} });
		builder.AddProduct(AD_OFF, ProductType.NonConsumable, new IDs(){{ AD_OFF, AppleAppStore.Name}, { AD_OFF, GooglePlay.Name} });

		UnityPurchasing.Initialize(this, builder);
	}

	/// <summary>
	/// iOS Specific.
	/// This is called as part of Apple's 'Ask to buy' functionality,
	/// when a purchase is requested by a minor and referred to a parent
	/// for approval.
	/// 
	/// When the purchase is approved or rejected, the normal purchase events
	/// will fire.
	/// </summary>
	/// <param name="item">Item.</param>
	private void OnDeferred(Product item)
	{
		Debug.Log("Purchase deferred: " + item.definition.id);
	}


	private bool IsInitialized()
	{
		Debug.Log("IsInitialized==================");
		Debug.Log("m_StoreController: " + (m_StoreController == null));
		Debug.Log("m_StoreExtensionProvider: " + (m_StoreExtensionProvider == null));
		return m_StoreController != null && m_StoreExtensionProvider != null;
	}

	public void BuyGems5()
	{
		BuyProductID(GEMS_5);
	}
	public void BuyGems30()
	{
		BuyProductID(GEMS_30);
	}
	public void BuyGems70()
	{
		BuyProductID(GEMS_70);
	}
	public void BuyGems150()
	{
		BuyProductID(GEMS_150);
	}
	public void BuyGems999()
	{
		BuyProductID(GEMS_999);
	}
	public void BuyAdOff()
	{
		BuyProductID(AD_OFF);		
	}


	void BuyProductID(string productId)
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
				Debug.Log("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
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
			return;
		}

		if (Application.platform == RuntimePlatform.IPhonePlayer || 
			Application.platform == RuntimePlatform.OSXPlayer)
		{
			Debug.Log("RestorePurchases started ...");

			var apple = m_StoreExtensionProvider.GetExtension<IAppleExtensions>();
			apple.RestoreTransactions((result) => {
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

		Debug.Log("Available items:");
		foreach (var item in controller.products.all)
		{
			if (item.availableToPurchase)
			{
				Debug.Log(string.Join(" - ",
					new[]
					{
						item.metadata.localizedTitle,
						item.metadata.localizedDescription,
						item.metadata.isoCurrencyCode,
						item.metadata.localizedPrice.ToString(),
						item.metadata.localizedPriceString,
						item.transactionID,
						item.receipt
					}));
			}
		}

		// On Apple platforms we need to handle deferred purchases caused by Apple's Ask to Buy feature.
		// On non-Apple platforms this will have no effect; OnDeferred will never be called.
		m_StoreExtensionProvider.GetExtension<IAppleExtensions>().RegisterPurchaseDeferredListener(OnDeferred);
	}

	public void OnInitializeFailed(InitializationFailureReason error)
	{
		Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
	}

	public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args) 
	{
		Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
		if (String.Equals(args.purchasedProduct.definition.id, GEMS_5, StringComparison.Ordinal))
		{
			ProgressManager.progressManager.playerData.Gems += 5;
		}
		else if (String.Equals(args.purchasedProduct.definition.id, GEMS_30, StringComparison.Ordinal))
		{
			ProgressManager.progressManager.playerData.Gems += 30;
		}
		else if (String.Equals(args.purchasedProduct.definition.id, GEMS_70, StringComparison.Ordinal))
		{
			ProgressManager.progressManager.playerData.Gems += 70;
		}
		else if (String.Equals(args.purchasedProduct.definition.id, GEMS_150, StringComparison.Ordinal))
		{
			ProgressManager.progressManager.playerData.Gems += 150;
		}
		else if (String.Equals(args.purchasedProduct.definition.id, GEMS_999, StringComparison.Ordinal))
		{
			ProgressManager.progressManager.playerData.Gems += 999;
		}
		else if (String.Equals(args.purchasedProduct.definition.id, AD_OFF, StringComparison.Ordinal))
		{
			ProgressManager.progressManager.playerData.IsAdOff = true;
		}
		ProgressManager.progressManager.Save ();

		SceneManager.LoadScene("MainScene", LoadSceneMode.Single);
		//playScreen.UpdateUI ();

		return PurchaseProcessingResult.Complete;
	}


	public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
	{
		Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));
	}
}