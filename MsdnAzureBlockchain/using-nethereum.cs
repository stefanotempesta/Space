// STEP 1: Obtain the contract ABI
abi = db.GetContract(agreement.ContractID);

if (string.IsNullOrEmpty(abi))
{
    Contracts coreAPIClient = new Contracts(new BChainCoreAPIs());
    ClientCredential clientCredential = new ClientCredential(
        Startup.ClientId,
        Startup.AppSecret);
    AuthenticationContext authContext = new AuthenticationContext(Startup.Authority);
    AuthenticationResult result = await authContext.AcquireTokenAsync(
        Startup.CoreAPIRecourceId,
        clientCredential);

    try
    {
        // Get the contract ABI from the core instance and save a copy locally
        abi = coreAPIClient.Get(agreement.ContractID, result.AccessToken);
    }
    catch (Exception ex)
    {
        Debug.WriteLine(ex);
    }

    if (string.IsNullOrEmpty(abi))
    {
        success = false;
    }
    else
    {
        success = (db.AddContract(agreement.ContractID, abi) > 0);
    }
}

// STEP 2: Get the function address to call on the smart contract
if (success)
{
    try
    {
        func = web3.Eth.GetContract(
            abi,
            agreement.ContractID).GetFunction("CreateProposal");
    }
    catch (Exception ex)
    {
        Debug.WriteLine(ex);
    }

    success = (func != null);
}

// STEP 3: Unlock the account so we can call the smart contract
if (success)
{
    string passphrase = db.GetAccountPassphrase(agreement.OriginatorAccount);

    try
    {
        success = await web3.Personal.UnlockAccount.SendRequestAsync(
            agreement.OriginatorAccount,
            passphrase,
            120);
    }
    catch (Exception ex)
    {
        success = false;
        Debug.WriteLine(ex);
    }
}

// STEP 4: Make the smart contract call
if (success)
{
    object[] args = new object[] {
        id,
        agreement.OriginatorAccount,
        agreement.CounterSigAccount };

    try
    {
        // Call the CreateProposal function on the smart contract
        await func.SendTransactionAsync(agreement.OriginatorAccount, args);
    }
    catch (Exception ex)
    {
        success = false;
        Debug.WriteLine(ex);
    }
}