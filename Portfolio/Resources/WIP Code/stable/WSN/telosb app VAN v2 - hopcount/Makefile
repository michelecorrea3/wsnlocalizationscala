COMPONENT=SenselessAppC
CFLAGS += -I$(TOSDIR)/lib/net -I$(TOSDIR)/lib/net/ctp -I$(TOSDIR)/lib/net/le -I$(TOSDIR)/lib/printf

BUILD_EXTRA_DEPS = CollMsg.class StatMsg.class LocMsg.class DissMsg.class
MIGFILES = CollMsg.java CollMsg.class StatMsg.java StatMsg.class LocMsg.java LocMsg.class DissMsg.java DissMsg.class

%.class:%.java
	javac $<

CollMsg.class: CollMsg.java
	javac CollMsg.java
CollMsg.java: Senseless.h
	mig java -target=null -java-classname=CollMsg Senseless.h CollMsg -o $@
	
StatMsg.class: StatMsg.java
	javac StatMsg.java
StatMsg.java: Senseless.h
	mig java -target=null -java-classname=StatMsg Senseless.h StatMsg -o $@
	
LocMsg.class: LocMsg.java
	javac LocMsg.java
LocMsg.java: Senseless.h
	mig java -target=null -java-classname=LocMsg Senseless.h LocMsg -o $@

DissMsg.class: DissMsg.java
	javac DissMsg.java
DissMsg.java: Senseless.h
	mig java -target=null -java-classname=DissMsg Senseless.h DissMsg -o $@

include $(MAKERULES)

migclean:
	rm -rf $(MIGFILES)