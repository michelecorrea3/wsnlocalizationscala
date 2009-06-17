COMPONENT=BasicNetworkingAppC
CFLAGS += -I$(TOSDIR)/lib/net -I$(TOSDIR)/lib/net/ctp -I$(TOSDIR)/lib/net/le -I$(TOSDIR)/lib/printf

BUILD_EXTRA_DEPS = CollMsg.class StatMsg.class LocMsg.class DissMsg.class
MIGFILES = CollMsg.java CollMsg.class StatMsg.java StatMsg.class LocMsg.java LocMsg.class DissMsg.java DissMsg.class

%.class:%.java
	javac $<

CollMsg.class: CollMsg.java
	javac CollMsg.java
CollMsg.java: BasicNetworking.h
	mig java -target=null -java-classname=CollMsg BasicNetworking.h CollMsg -o $@
	
StatMsg.class: StatMsg.java
	javac StatMsg.java
StatMsg.java: BasicNetworking.h
	mig java -target=null -java-classname=StatMsg BasicNetworking.h StatMsg -o $@
	
LocMsg.class: LocMsg.java
	javac LocMsg.java
LocMsg.java: BasicNetworking.h
	mig java -target=null -java-classname=LocMsg BasicNetworking.h LocMsg -o $@

DissMsg.class: DissMsg.java
	javac DissMsg.java
DissMsg.java: BasicNetworking.h
	mig java -target=null -java-classname=DissMsg BasicNetworking.h DissMsg -o $@

include $(MAKERULES)

migclean:
	rm -rf $(MIGFILES)