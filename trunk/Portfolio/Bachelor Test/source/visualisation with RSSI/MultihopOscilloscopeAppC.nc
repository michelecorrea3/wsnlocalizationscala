#include "MultihopOscilloscope.h"
#include "printf.h"
configuration MultihopOscilloscopeAppC 
{ 
}
implementation 
{
	//Miscalleny
	//--------------------------------------------------------------------------------------------------------------------------
		components MainC;
		components MultihopOscilloscopeC;
		components LedsC;
		components new TimerMilliC(); 

  
		MultihopOscilloscopeC.Boot -> MainC;
		MultihopOscilloscopeC.Timer -> TimerMilliC;
		MultihopOscilloscopeC.Leds -> LedsC;
  
	//Printf:
	// ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
		components PrintfC;
		MultihopOscilloscopeC.PrintfControl -> PrintfC;
		MultihopOscilloscopeC.PrintfFlush -> PrintfC;
		
	// Broadcast
	//-----------------------------------------------------------------------------------------------------------------------------------------
//		components new AMSenderC(AM_OSCILLOSCOPE) as AnchorSender;
		components new AMReceiverC(AM_OSCILLOSCOPE) as MobileReceiver;
		
		MultihopOscilloscopeC.MobileReceive -> MobileReceiver;
//		MultihopOscilloscopeC.AMControl -> ActiveMessageC;
//		MultihopOscilloscopeC.AnchorSend -> AnchorSender;
		
		
	//RSSI
	//---------------------------------------------------------------------------------------------------------------------------------------
		components CC2420ActiveMessageC as Radio;
		
		MultihopOscilloscopeC -> Radio.CC2420Packet;		
		
	// Sensors
	// ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
		components new DemoSensorC() as SensorVoltage; 			 //This is used for measuring the internal Voltage
		components new HamamatsuS1087ParC() as SensorLight;  	 //Sensor for measuring the light intensity
		components new SensirionSht11C() as SensorTempHumidity;  //Sensor for measuring the temperature and humidity 

		MultihopOscilloscopeC.ReadVoltage -> SensorVoltage;
		MultihopOscilloscopeC.ReadLight -> SensorLight;
		MultihopOscilloscopeC.ReadTemperature -> SensorTempHumidity.Temperature;
		MultihopOscilloscopeC.ReadHumidity -> SensorTempHumidity.Humidity;
		
		
	//Seriele communicatie
	//---------------------------------------------------------------------------------------------------------------------------------------
		components SerialActiveMessageC;			//serial messaging
		components new SerialAMSenderC(AM_OSCILLOSCOPE);	// Sends to the serial port (generic configuration)	

		MultihopOscilloscopeC.SerialSend -> SerialAMSenderC.AMSend;
		MultihopOscilloscopeC.SerialControl -> SerialActiveMessageC;
		
		
	// General Radio Communication
	//-----------------------------------------------------------------------------------------------------------------------------------------
		components ActiveMessageC;		//AM LAAG	
		
		MultihopOscilloscopeC.RadioControl -> ActiveMessageC;
		
		
	// Collected Radio Communication
	//---------------------------------------------------------------------------------------------------------------------------------------
//	#if defined(COLLECTION_PROTOCOL)
		components CtpP as CollectP;	
		components CollectionC as Collector;  // Collection layer
		components new CollectionSenderC(AM_OSCILLOSCOPE); // Sends multihop RF
  
		MultihopOscilloscopeC.RoutingControl -> Collector;
		MultihopOscilloscopeC.CollectSend -> CollectionSenderC;
		MultihopOscilloscopeC.Snoop -> Collector.Snoop[AM_OSCILLOSCOPE];
		MultihopOscilloscopeC.CollectReceive -> Collector.Receive[AM_OSCILLOSCOPE];
		MultihopOscilloscopeC.RootControl -> Collector;
		
//	#elif defined(DUMMY_COLLECT_PROTOCOL)
//		components DummyP as CollectP;		
// 	#else
//			#error "A protocol needs to be selected to collect data"
//	#endif
		MultihopOscilloscopeC.CollectInfo -> CollectP;

	// Memory
	//----------------------------------------------------------------------------------------------------------------------------------------
		components new PoolC(message_t, 10) as UARTMessagePoolP;
		components new QueueC(message_t*, 10) as UARTQueueP;

		MultihopOscilloscopeC.UARTMessagePool -> UARTMessagePoolP;
		MultihopOscilloscopeC.UARTQueue -> UARTQueueP;
 
 // Memory
	//----------------------------------------------------------------------------------------------------------------------------------------
		components new PoolC(message_t, 10) as RadioMessagePoolP;
		components new QueueC(message_t*, 10) as RadioQueueP;

		MultihopOscilloscopeC.RadioMessagePool -> RadioMessagePoolP;
		MultihopOscilloscopeC.RadioQueue -> RadioQueueP;
		
		
	// Debug
	//---------------------------------------------------------------------------------------------------------------------------------------
 
		components new PoolC(message_t, 20) as DebugMessagePool;
		components new QueueC(message_t*, 20) as DebugSendQueue;
		components new SerialAMSenderC(AM_CTP_DEBUG) as DebugSerialSender,
		UARTDebugSenderP as DebugSender;

		DebugSender.Boot -> MainC;
		DebugSender.UARTSend -> DebugSerialSender;
		DebugSender.MessagePool -> DebugMessagePool;
		DebugSender.SendQueue -> DebugSendQueue;
		Collector.CollectionDebug -> DebugSender;
}
