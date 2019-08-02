# Telemetry Smart Contract
The Telemetry smart contract implemented in Solidity programming language for Azure Blockchain Workbench,
defines parties, properties and states, which all together are a representation of the status of the contract at any given time.
Supply chain parties are identified by a blockchain address.
In Azure Blockchain Workbench, this is the user or device address as registered on the platform.
States represents an enumeration of stages in the supply chain process 
(contract created, goods in transit, transaction completed, or out of compliance recorded), 
as well as the two different types of sensors in use in this example (humidity and temperature). 
Enumeration allow for more entries to be added easily, should the supply chain process require additional stages 
or introduce different sensor types.

### Constructor
The UI for entering a new contract in Blockchain Workbench is generated automatically 
based on the constructor of the Telemetry smart contract.
This constructor sets the initial conditions for the transportation process
that have to be met by all parties involved in the supply chain.
It also set the initial state to StateType.Created.

### Read Telemetry
When telemetry data is streamed by an IoT device and a rule exception occurs in Azure IoT Central, 
a workflow is executed, which in turn sends a message to Blockchain Workbench to invoke the ReadTelemetry function.
This function validates the input values for humidity or temperature recorded by the IoT device, 
and raises an out of compliance state if the conditions set in the first stage aren’t met.
