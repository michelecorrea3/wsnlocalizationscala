#include "BasicNetworking.h"
 
configuration BasicNetworkingAppC { }
implementation {
  // other
 components MainC;
  components BasicNetworkingC;
  components LedsC;
  components new TimerMilliC(), new TimerMilliC() as BroadCastTimerMilliC;
  
  BasicNetworkingC.Boot->MainC;
  BasicNetworkingC.Timer->TimerMilliC;
  BasicNetworkingC.BroadcastTimer->BroadCastTimerMilliC;
  BasicNetworkingC.Leds->LedsC;
  
  // sensors
  components new HamamatsuS1087ParC() as LightSensor;
  components new SensirionSht11C() as TempHumiditySensor;
  components new DemoSensorC() as VoltageSensor;
  
  BasicNetworkingC.ReadLight->LightSensor;
  BasicNetworkingC.ReadTemp->TempHumiditySensor.Temperature;
  BasicNetworkingC.ReadHumidity->TempHumiditySensor.Humidity;
  BasicNetworkingC.ReadBattery->VoltageSensor;
  
  // button
  components UserButtonC;
  
  BasicNetworkingC.Notify->UserButtonC;
  
  // serial
  components SerialActiveMessageC;
  components new SerialAMReceiverC(AM_DISSMSG) as SerialRequestReceiver;
  components new SerialAMSenderC(AM_COLLMSG) as SerialCollectSender;
  
  BasicNetworkingC.UartPacket->SerialActiveMessageC;
  BasicNetworkingC.SerialControl->SerialActiveMessageC;
  BasicNetworkingC.SerialSend->SerialCollectSender.AMSend;
  BasicNetworkingC.SerialReceive->SerialRequestReceiver;
  
  //RSSI
  components CC2420ActiveMessageC as Radio;
  
  BasicNetworkingC.CC2420Packet -> Radio;
  
  // general radio Communication
  components ActiveMessageC;
  
  BasicNetworkingC.RadioControl->ActiveMessageC; 
  BasicNetworkingC.RadioPacket->ActiveMessageC;  
  
  //Broadcast
  components new AMReceiverC(AM_OSCILLOSCOPE) as BroadcastReceiver;
  components new AMSenderC(AM_OSCILLOSCOPE) as BroadcastSender;
  
  BasicNetworkingC.BroadcastReceive->BroadcastReceiver;
  BasicNetworkingC.BroadcastSend -> BroadcastSender;
  
  // Collection
  components CollectionC as Collector; // Collection
  components new CollectionSenderC(AM_COLLMSG); // multihop RF
  
  BasicNetworkingC.RoutingControl->Collector;
  BasicNetworkingC.CollectSend->CollectionSenderC;
  BasicNetworkingC.Snoop->Collector.Snoop[AM_COLLMSG];
  BasicNetworkingC.CollectReceive->Collector.Receive[AM_COLLMSG];
  BasicNetworkingC.RootControl->Collector;
  
  // Dissemination
  components DisseminationC;
  components new DisseminatorC(DissMsg_t, BROADCAST_DIS_KEY);
  
  BasicNetworkingC.DisseminationControl->DisseminationC;
  BasicNetworkingC.RequestUpdate->DisseminatorC;
  BasicNetworkingC.RequestValue->DisseminatorC;

  // Memory
  components new PoolC(message_t, 5) as RadioMessagePoolP;
  components new QueueC(message_t*, 5) as RadioQueueP;

  BasicNetworkingC.RadioPoolC->RadioMessagePoolP;
  BasicNetworkingC.RadioQueueC->RadioQueueP;
  BasicNetworkingC.RadioPoolS->RadioMessagePoolP;
  BasicNetworkingC.RadioQueueS->RadioQueueP;
  BasicNetworkingC.RadioPoolL->RadioMessagePoolP;
  BasicNetworkingC.RadioQueueL->RadioQueueP;
  
  components new PoolC(message_t, 5) as UartMessagePoolP;
  components new QueueC(message_t*, 5) as UartQueueP;

  BasicNetworkingC.UartPoolC->UartMessagePoolP;
  BasicNetworkingC.UartQueueC->UartQueueP;
  BasicNetworkingC.UartPoolS->UartMessagePoolP;
  BasicNetworkingC.UartQueueS->UartQueueP;
  BasicNetworkingC.UartPoolL->UartMessagePoolP;
  BasicNetworkingC.UartQueueL->UartQueueP;
  // printf()
  components PrintfC;

  BasicNetworkingC.PrintfControl->PrintfC;
  BasicNetworkingC.PrintfFlush->PrintfC;
}