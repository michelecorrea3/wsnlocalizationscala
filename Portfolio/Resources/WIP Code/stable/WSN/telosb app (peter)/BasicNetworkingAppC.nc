#include "BasicNetworking.h"
 
configuration BasicNetworkingAppC { }
implementation {
  // other
  components MainC;
  components BasicNetworkingC;
  components LedsC;
  components new TimerMilliC();
  
  BasicNetworkingC.Boot->MainC;
  BasicNetworkingC.Timer->TimerMilliC;
  BasicNetworkingC.BroadcastTimer->TimerMilliC;
  BasicNetworkingC.Leds->LedsC;
  
  // sensors
  components new HamamatsuS1087ParC() as LightSensor;
  components new SensirionSht11C() as TempHumiditySensor;
  
  BasicNetworkingC.ReadLight->LightSensor;
  BasicNetworkingC.ReadTemp->TempHumiditySensor.Temperature;
  BasicNetworkingC.ReadHumidity->TempHumiditySensor.Humidity;
  
  // button
  components UserButtonC;
  
  BasicNetworkingC.Notify->UserButtonC;
  
  // serial
  components SerialActiveMessageC;
  components new SerialAMReceiverC(AM_BN_DISSMSG) as SerialRequestReceiver;
  components new SerialAMSenderC(AM_BN_COLLMSG) as SerialCollectSender;
  
  BasicNetworkingC.UartPacket->SerialActiveMessageC;
  BasicNetworkingC.SerialControl->SerialActiveMessageC;
  BasicNetworkingC.SerialSend->SerialCollectSender.AMSend;
  BasicNetworkingC.SerialReceive->SerialRequestReceiver;
  
  // general radio
  components ActiveMessageC, new AMReceiverC(AM_BN_COLLMSG),
  CC2420ActiveMessageC as Radio, new AMSenderC(AM_OSCILLOSCOPE);
  
  BasicNetworkingC.RadioPacket->ActiveMessageC;
  BasicNetworkingC.Receive->AMReceiverC;
  BasicNetworkingC.RadioControl->ActiveMessageC;
  BasicNetworkingC.CC2420Packet -> Radio;
  BasicNetworkingC.AMSend -> AMSenderC;
  
  // Collection
  components CollectionC as Collector; // Collection
  components new CollectionSenderC(AM_BN_COLLMSG); // multihop RF
  
  BasicNetworkingC.RoutingControl->Collector;
  BasicNetworkingC.CollectSend->CollectionSenderC;
  BasicNetworkingC.Snoop->Collector.Snoop[AM_BN_COLLMSG];
  BasicNetworkingC.CollectReceive->Collector.Receive[AM_BN_COLLMSG];
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

  BasicNetworkingC.RadioPool->RadioMessagePoolP;
  BasicNetworkingC.RadioQueue->RadioQueueP;

  components new PoolC(message_t, 5) as UartMessagePoolP;
  components new QueueC(message_t*, 5) as UartQueueP;

  BasicNetworkingC.UartPool->UartMessagePoolP;
  BasicNetworkingC.UartQueue->UartQueueP;

  // printf()
  components PrintfC;

  BasicNetworkingC.PrintfControl->PrintfC;
  BasicNetworkingC.PrintfFlush->PrintfC;
}