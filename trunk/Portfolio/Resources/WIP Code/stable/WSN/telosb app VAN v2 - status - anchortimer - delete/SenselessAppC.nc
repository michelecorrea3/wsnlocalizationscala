#include "Senseless.h"
 
configuration SenselessAppC { }
implementation {
  // Other
  //---------------------------------------------------------------------------
  components MainC;
  components SenselessC;
  components LedsC;
  components new TimerMilliC(), new TimerMilliC() as BroadCastTimerMilliC, new TimerMilliC() as AnchortimerMilliC;
  
  SenselessC.Boot->MainC;
  SenselessC.Timer->TimerMilliC;
  SenselessC.Anchortimer->AnchortimerMilliC;
  SenselessC.BroadcastTimer->BroadCastTimerMilliC;
  SenselessC.Leds->LedsC;
  
  // Sensors
  //---------------------------------------------------------------------------
  components new HamamatsuS1087ParC() as LightSensor;
  components new SensirionSht11C() as TempHumiditySensor;
  components new DemoSensorC() as VoltageSensor;
  
  SenselessC.ReadLight->LightSensor;
  SenselessC.ReadTemp->TempHumiditySensor.Temperature;
  SenselessC.ReadHumidity->TempHumiditySensor.Humidity;
  SenselessC.ReadBattery->VoltageSensor;
  
  // Button
  //---------------------------------------------------------------------------
  components UserButtonC;
  
  SenselessC.Notify->UserButtonC;
  
  // Serial
  //---------------------------------------------------------------------------
  components SerialActiveMessageC;
  components new SerialAMReceiverC(AM_DISSMSG) as SerialRequestReceiver;
  components new SerialAMSenderC(AM_COLLMSG) as SerialCollectSenderC,
			 new SerialAMSenderC(AM_LOCMSG) as SerialCollectSenderL,
			 new SerialAMSenderC(AM_STATMSG) as SerialCollectSenderS;
  
  SenselessC.UartPacket->SerialActiveMessageC;
  SenselessC.SerialControl->SerialActiveMessageC;
  SenselessC.SerialSendC->SerialCollectSenderC.AMSend;
  SenselessC.SerialSendL->SerialCollectSenderL.AMSend;
  SenselessC.SerialSendS->SerialCollectSenderS.AMSend;
  SenselessC.SerialReceive->SerialRequestReceiver;
  
  //RSSI & Frequency & Powerlevel
  //---------------------------------------------------------------------------
  components CC2420ActiveMessageC as Radio;
  components CC2420ControlC;
  
  SenselessC.CC2420Packet -> Radio;
  SenselessC.CC2420Config -> CC2420ControlC;
  
  // General radio Communication
  //---------------------------------------------------------------------------
  components ActiveMessageC;
  
  SenselessC.RadioControl->ActiveMessageC; 
  SenselessC.RadioPacket->ActiveMessageC;  
  
  // Broadcast
  //---------------------------------------------------------------------------
  components new AMReceiverC(AM_OSCILLOSCOPE) as BroadcastReceiver;
  components new AMSenderC(AM_OSCILLOSCOPE) as BroadcastSender;
  
  SenselessC.BroadcastReceive->BroadcastReceiver;
  SenselessC.BroadcastSend -> BroadcastSender;
  
  // Collection
  //---------------------------------------------------------------------------
  components CollectionC as CollectorC,
			CollectionC as CollectorL,
			CollectionC as CollectorS; // Collection
  components new CollectionSenderC(AM_COLLMSG) as ColSendC, 
			 new CollectionSenderC(AM_LOCMSG) as ColSendL, 
			 new CollectionSenderC(AM_STATMSG) as ColSendS; // multihop RF
  
  SenselessC.RoutingControlC->CollectorC;
  SenselessC.RoutingControlL->CollectorL;
  SenselessC.RoutingControlS->CollectorS;
  SenselessC.CollectSendC->ColSendC;
  SenselessC.CollectSendL->ColSendL;
  SenselessC.CollectSendS->ColSendS;
  SenselessC.SnoopC->CollectorC.Snoop[AM_COLLMSG];
  SenselessC.SnoopL->CollectorL.Snoop[AM_LOCMSG];
  SenselessC.SnoopS->CollectorS.Snoop[AM_STATMSG];
  SenselessC.CollectReceiveC->CollectorC.Receive[AM_COLLMSG];
  SenselessC.CollectReceiveL->CollectorL.Receive[AM_LOCMSG];
  SenselessC.CollectReceiveS->CollectorS.Receive[AM_STATMSG];
  SenselessC.RootControlC->CollectorC;
  SenselessC.RootControlL->CollectorL;
  SenselessC.RootControlS->CollectorS;
  
  // Dissemination
  //---------------------------------------------------------------------------
  components DisseminationC;
  components new DisseminatorC(DissMsg_t, BROADCAST_DIS_KEY);
  
  SenselessC.DisseminationControl->DisseminationC;
  SenselessC.RequestUpdate->DisseminatorC;
  SenselessC.RequestValue->DisseminatorC;

  // Memory (for the 3 different messages)
  //---------------------------------------------------------------------------
  components new PoolC(message_t, 5) as RadioMessagePoolP;
  components new QueueC(message_t*, 5) as RadioQueueP;

  SenselessC.RadioPoolC->RadioMessagePoolP;
  SenselessC.RadioQueueC->RadioQueueP;
  SenselessC.RadioPoolS->RadioMessagePoolP;
  SenselessC.RadioQueueS->RadioQueueP;
  SenselessC.RadioPoolL->RadioMessagePoolP;
  SenselessC.RadioQueueL->RadioQueueP;
  
  components new PoolC(message_t, 5) as UartMessagePoolP;
  components new QueueC(message_t*, 5) as UartQueueP;

  SenselessC.UartPoolC->UartMessagePoolP;
  SenselessC.UartQueueC->UartQueueP;
  SenselessC.UartPoolS->UartMessagePoolP;
  SenselessC.UartQueueS->UartQueueP;
  SenselessC.UartPoolL->UartMessagePoolP;
  SenselessC.UartQueueL->UartQueueP;
  
  // Printf
  //---------------------------------------------------------------------------
  components PrintfC;

  SenselessC.PrintfControl->PrintfC;
  SenselessC.PrintfFlush->PrintfC;
}