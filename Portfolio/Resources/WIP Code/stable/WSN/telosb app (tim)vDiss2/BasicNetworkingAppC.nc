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
  components new SerialAMSenderC(AM_COLLMSG) as SerialCollectSenderC,
			 new SerialAMSenderC(AM_LOCMSG) as SerialCollectSenderL,
			 new SerialAMSenderC(AM_STATMSG) as SerialCollectSenderS;
  
  BasicNetworkingC.UartPacket->SerialActiveMessageC;
  BasicNetworkingC.SerialControl->SerialActiveMessageC;
  BasicNetworkingC.SerialSendC->SerialCollectSenderC.AMSend;
  BasicNetworkingC.SerialSendL->SerialCollectSenderL.AMSend;
  BasicNetworkingC.SerialSendS->SerialCollectSenderS.AMSend;
  BasicNetworkingC.SerialReceive->SerialRequestReceiver;
  
  //RSSI
  components CC2420ActiveMessageC as Radio;
  
  BasicNetworkingC.CC2420Packet -> Radio;
  //BasicNetworkingC.CC2420Config -> Radio;
  
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
  components CollectionC as CollectorC,
			CollectionC as CollectorL,
			CollectionC as CollectorS; // Collection
  components new CollectionSenderC(AM_COLLMSG) as ColSendC, 
			 new CollectionSenderC(AM_LOCMSG) as ColSendL, 
			 new CollectionSenderC(AM_STATMSG) as ColSendS; // multihop RF
  
  BasicNetworkingC.RoutingControlC->CollectorC;
  BasicNetworkingC.RoutingControlL->CollectorL;
  BasicNetworkingC.RoutingControlS->CollectorS;
  BasicNetworkingC.CollectSendC->ColSendC;
  BasicNetworkingC.CollectSendL->ColSendL;
  BasicNetworkingC.CollectSendS->ColSendS;
  BasicNetworkingC.SnoopC->CollectorC.Snoop[AM_COLLMSG];
  BasicNetworkingC.SnoopL->CollectorL.Snoop[AM_LOCMSG];
  BasicNetworkingC.SnoopS->CollectorS.Snoop[AM_STATMSG];
  BasicNetworkingC.CollectReceiveC->CollectorC.Receive[AM_COLLMSG];
  BasicNetworkingC.CollectReceiveL->CollectorL.Receive[AM_LOCMSG];
  BasicNetworkingC.CollectReceiveS->CollectorS.Receive[AM_STATMSG];
  BasicNetworkingC.RootControlC->CollectorC;
  BasicNetworkingC.RootControlL->CollectorL;
  BasicNetworkingC.RootControlS->CollectorS;
  
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