pragma solidity >=0.4.25 <0.6.0;

contract Telemetry
{
    // States
    enum StateType { Created, InTransit, Completed, OutOfCompliance }
    enum SensorType { None, Humidity, Temperature }

    // Parties
    address public Owner;
    address public InitiatingCounterparty;
    address public Counterparty;
    address public PreviousCounterparty;
    address public Device;
    address public SupplyChainOwner;
    address public SupplyChainObserver;
    
    // Properties
    StateType public State;
    SensorType public ComplianceSensorType;
    int public MinHumidity;
    int public MaxHumidity;
    int public MinTemperature;
    int public MaxTemperature;
    int public ComplianceSensorReading;
    bool public ComplianceStatus;
    string public ComplianceDetail;
    int public LastSensorUpdateTimestamp;

    constructor(address device, address supplyChainOwner, address supplyChainObserver, int minHumidity, int maxHumidity, int minTemperature, int maxTemperature) public
    {
        ComplianceStatus = true;
        ComplianceSensorReading = -1;
        InitiatingCounterparty = msg.sender;
        Owner = InitiatingCounterparty;
        Counterparty = InitiatingCounterparty;
        Device = device;
        SupplyChainOwner = supplyChainOwner;
        SupplyChainObserver = supplyChainObserver;
        MinHumidity = minHumidity;
        MaxHumidity = maxHumidity;
        MinTemperature = minTemperature;
        MaxTemperature = maxTemperature;
        State = StateType.Created;
        ComplianceDetail = "N/A";
    }

    function ReadTelemetry(int humidity, int temperature, int timestamp) public
    {
        // Separately check for states and sender 
        // to avoid not checking for state when the sender is the device
        // because of the logical OR
        if (State == StateType.Completed || State == StateType.OutOfCompliance)
        {
            revert();
        }

        if (Device != msg.sender)
        {
            revert();
        }

        LastSensorUpdateTimestamp = timestamp;

        if (humidity < MinHumidity || humidity > MaxHumidity)
        {
            ComplianceSensorType = SensorType.Humidity;
            ComplianceSensorReading = humidity;
            ComplianceDetail = "Humidity value out of range.";
            ComplianceStatus = false;
        }
        else if (temperature < MinTemperature || temperature > MaxTemperature)
        {
            ComplianceSensorType = SensorType.Temperature;
            ComplianceSensorReading = temperature;
            ComplianceDetail = "Temperature value out of range.";
            ComplianceStatus = false;
        }

        if (ComplianceStatus == false)
        {
            State = StateType.OutOfCompliance;
        }
    }

    function TransferResponsibility(address newCounterparty) public
    {
        // keep the state checking, message sender, and device checks separate
        // to not get cloberred by the order of evaluation for logical OR
        if (State == StateType.Completed)
        {
            revert();
        }

        if (State == StateType.OutOfCompliance)
        {
            revert();
        }

        if (InitiatingCounterparty != msg.sender && Counterparty != msg.sender)
        {
            revert();
        }

        if (newCounterparty == Device)
        {
            revert();
        }

        if (State == StateType.Created)
        {
            State = StateType.InTransit;
        }

        PreviousCounterparty = Counterparty;
        Counterparty = newCounterparty;
    }

    function Complete() public
    {
        // keep the state checking, message sender, and device checks separate
        // to not get cloberred by the order of evaluation for logical OR
        if (State == StateType.Completed)
        {
            revert();
        }

        if (State == StateType.OutOfCompliance)
        {
            revert();
        }

        if (Owner != msg.sender && SupplyChainOwner != msg.sender)
        {
            revert();
        }

        State = StateType.Completed;
        PreviousCounterparty = Counterparty;
        Counterparty = 0x0000000000000000000000000000000000000000;
    }
}