COMPONENT=BasicNetworkingAppC
CFLAGS += -I$(TOSDIR)/lib/net -I$(TOSDIR)/lib/net/ctp -I$(TOSDIR)/lib/net/le -I$(TOSDIR)/lib/printf

BUILD_EXTRA_DEPS = BN_CollMsg.class BN_DissMsg.class
MIGFILES = BN_CollMsg.java BN_CollMsg.class BN_DissMsg.java BN_DissMsg.class

%.class:%.java
	javac $<

BN_CollMsg.class: BN_CollMsg.java
	javac BN_CollMsg.java
BN_CollMsg.java: BasicNetworking.h
	mig java -target=null -java-classname=BN_CollMsg BasicNetworking.h BN_CollMsg -o $@

BN_DissMsg.class: BN_DissMsg.java
	javac BN_DissMsg.java
BN_DissMsg.java: BasicNetworking.h
	mig java -target=null -java-classname=BN_DissMsg BasicNetworking.h BN_DissMsg -o $@

include $(MAKERULES)

migclean:
	rm -rf $(MIGFILES)