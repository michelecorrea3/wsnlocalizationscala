/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

package parserxmlnew;

/**
 *
 * @author Poseidon
 */
public class LocMsg extends net.tinyos.message.Message {

    /** The default size of this message type in bytes. */
    public static final int DEFAULT_MESSAGE_SIZE = 10;

    /** The Active Message type associated with this message. */
    public static final int AM_TYPE = -1;

    /** Create a new LocMsg of size 10. */
    public LocMsg() {
        super(DEFAULT_MESSAGE_SIZE);
        amTypeSet(AM_TYPE);
    }

    /** Create a new LocMsg of the given data_length. */
    public LocMsg(int data_length) {
        super(data_length);
        amTypeSet(AM_TYPE);
    }

    /**
     * Create a new LocMsg with the given data_length
     * and base offset.
     */
    public LocMsg(int data_length, int base_offset) {
        super(data_length, base_offset);
        amTypeSet(AM_TYPE);
    }

    /**
     * Create a new LocMsg using the given byte array
     * as backing store.
     */
    public LocMsg(byte[] data) {
        super(data);
        amTypeSet(AM_TYPE);
    }

    /**
     * Create a new LocMsg using the given byte array
     * as backing store, with the given base offset.
     */
    public LocMsg(byte[] data, int base_offset) {
        super(data, base_offset);
        amTypeSet(AM_TYPE);
    }

    /**
     * Create a new LocMsg using the given byte array
     * as backing store, with the given base offset and data length.
     */
    public LocMsg(byte[] data, int base_offset, int data_length) {
        super(data, base_offset, data_length);
        amTypeSet(AM_TYPE);
    }

    /**
     * Create a new LocMsg embedded in the given message
     * at the given base offset.
     */
    public LocMsg(net.tinyos.message.Message msg, int base_offset) {
        super(msg, base_offset, DEFAULT_MESSAGE_SIZE);
        amTypeSet(AM_TYPE);
    }

    /**
     * Create a new LocMsg embedded in the given message
     * at the given base offset and length.
     */
    public LocMsg(net.tinyos.message.Message msg, int base_offset, int data_length) {
        super(msg, base_offset, data_length);
        amTypeSet(AM_TYPE);
    }

    /**
    /* Return a String representation of this message. Includes the
     * message type name and the non-indexed field values.
     */
    public String toString() {
      String s = "Message <LocMsg> \n";
      try {
        s += "  [moteid=0x"+Long.toHexString(get_moteid())+"]\n";
      } catch (ArrayIndexOutOfBoundsException aioobe) { /* Skip field */ }
      try {
        s += "  [ANmoteid=0x"+Long.toHexString(get_ANmoteid())+"]\n";
      } catch (ArrayIndexOutOfBoundsException aioobe) { /* Skip field */ }
      try {
        s += "  [type=0x"+Long.toHexString(get_type())+"]\n";
      } catch (ArrayIndexOutOfBoundsException aioobe) { /* Skip field */ }
      try {
        s += "  [posx=0x"+Long.toHexString(get_posx())+"]\n";
      } catch (ArrayIndexOutOfBoundsException aioobe) { /* Skip field */ }
      try {
        s += "  [posy=0x"+Long.toHexString(get_posy())+"]\n";
      } catch (ArrayIndexOutOfBoundsException aioobe) { /* Skip field */ }
      try {
        s += "  [RSSI=0x"+Long.toHexString(get_RSSI())+"]\n";
      } catch (ArrayIndexOutOfBoundsException aioobe) { /* Skip field */ }
      return s;
    }

    // Message-type-specific access methods appear below.

    /////////////////////////////////////////////////////////
    // Accessor methods for field: moteid
    //   Field type: int, signed
    //   Offset (bits): 0
    //   Size (bits): 16
    /////////////////////////////////////////////////////////

    /**
     * Return whether the field 'moteid' is signed (true).
     */
    public static boolean isSigned_moteid() {
        return true;
    }

    /**
     * Return whether the field 'moteid' is an array (false).
     */
    public static boolean isArray_moteid() {
        return false;
    }

    /**
     * Return the offset (in bytes) of the field 'moteid'
     */
    public static int offset_moteid() {
        return (0 / 8);
    }

    /**
     * Return the offset (in bits) of the field 'moteid'
     */
    public static int offsetBits_moteid() {
        return 0;
    }

    /**
     * Return the value (as a int) of the field 'moteid'
     */
    public int get_moteid() {
        return (int)getUIntBEElement(offsetBits_moteid(), 16);
    }

    /**
     * Set the value of the field 'moteid'
     */
    public void set_moteid(int value) {
        setUIntBEElement(offsetBits_moteid(), 16, value);
    }

    /**
     * Return the size, in bytes, of the field 'moteid'
     */
    public static int size_moteid() {
        return (16 / 8);
    }

    /**
     * Return the size, in bits, of the field 'moteid'
     */
    public static int sizeBits_moteid() {
        return 16;
    }

    /////////////////////////////////////////////////////////
    // Accessor methods for field: ANmoteid
    //   Field type: int, signed
    //   Offset (bits): 16
    //   Size (bits): 16
    /////////////////////////////////////////////////////////

    /**
     * Return whether the field 'ANmoteid' is signed (true).
     */
    public static boolean isSigned_ANmoteid() {
        return true;
    }

    /**
     * Return whether the field 'ANmoteid' is an array (false).
     */
    public static boolean isArray_ANmoteid() {
        return false;
    }

    /**
     * Return the offset (in bytes) of the field 'ANmoteid'
     */
    public static int offset_ANmoteid() {
        return (16 / 8);
    }

    /**
     * Return the offset (in bits) of the field 'ANmoteid'
     */
    public static int offsetBits_ANmoteid() {
        return 16;
    }

    /**
     * Return the value (as a int) of the field 'ANmoteid'
     */
    public int get_ANmoteid() {
        return (int)getUIntBEElement(offsetBits_ANmoteid(), 16);
    }

    /**
     * Set the value of the field 'ANmoteid'
     */
    public void set_ANmoteid(int value) {
        setUIntBEElement(offsetBits_ANmoteid(), 16, value);
    }

    /**
     * Return the size, in bytes, of the field 'ANmoteid'
     */
    public static int size_ANmoteid() {
        return (16 / 8);
    }

    /**
     * Return the size, in bits, of the field 'ANmoteid'
     */
    public static int sizeBits_ANmoteid() {
        return 16;
    }

    /////////////////////////////////////////////////////////
    // Accessor methods for field: type
    //   Field type: short, signed
    //   Offset (bits): 32
    //   Size (bits): 8
    /////////////////////////////////////////////////////////

    /**
     * Return whether the field 'type' is signed (true).
     */
    public static boolean isSigned_type() {
        return true;
    }

    /**
     * Return whether the field 'type' is an array (false).
     */
    public static boolean isArray_type() {
        return false;
    }

    /**
     * Return the offset (in bytes) of the field 'type'
     */
    public static int offset_type() {
        return (32 / 8);
    }

    /**
     * Return the offset (in bits) of the field 'type'
     */
    public static int offsetBits_type() {
        return 32;
    }

    /**
     * Return the value (as a short) of the field 'type'
     */
    public short get_type() {
        return (short)getUIntBEElement(offsetBits_type(), 8);
    }

    /**
     * Set the value of the field 'type'
     */
    public void set_type(short value) {
        setUIntBEElement(offsetBits_type(), 8, value);
    }

    /**
     * Return the size, in bytes, of the field 'type'
     */
    public static int size_type() {
        return (8 / 8);
    }

    /**
     * Return the size, in bits, of the field 'type'
     */
    public static int sizeBits_type() {
        return 8;
    }

    /////////////////////////////////////////////////////////
    // Accessor methods for field: posx
    //   Field type: int, signed
    //   Offset (bits): 40
    //   Size (bits): 16
    /////////////////////////////////////////////////////////

    /**
     * Return whether the field 'posx' is signed (true).
     */
    public static boolean isSigned_posx() {
        return true;
    }

    /**
     * Return whether the field 'posx' is an array (false).
     */
    public static boolean isArray_posx() {
        return false;
    }

    /**
     * Return the offset (in bytes) of the field 'posx'
     */
    public static int offset_posx() {
        return (40 / 8);
    }

    /**
     * Return the offset (in bits) of the field 'posx'
     */
    public static int offsetBits_posx() {
        return 40;
    }

    /**
     * Return the value (as a int) of the field 'posx'
     */
    public int get_posx() {
        return (int)getUIntBEElement(offsetBits_posx(), 16);
    }

    /**
     * Set the value of the field 'posx'
     */
    public void set_posx(int value) {
        setUIntBEElement(offsetBits_posx(), 16, value);
    }

    /**
     * Return the size, in bytes, of the field 'posx'
     */
    public static int size_posx() {
        return (16 / 8);
    }

    /**
     * Return the size, in bits, of the field 'posx'
     */
    public static int sizeBits_posx() {
        return 16;
    }

    /////////////////////////////////////////////////////////
    // Accessor methods for field: posy
    //   Field type: int, signed
    //   Offset (bits): 56
    //   Size (bits): 16
    /////////////////////////////////////////////////////////

    /**
     * Return whether the field 'posy' is signed (true).
     */
    public static boolean isSigned_posy() {
        return true;
    }

    /**
     * Return whether the field 'posy' is an array (false).
     */
    public static boolean isArray_posy() {
        return false;
    }

    /**
     * Return the offset (in bytes) of the field 'posy'
     */
    public static int offset_posy() {
        return (56 / 8);
    }

    /**
     * Return the offset (in bits) of the field 'posy'
     */
    public static int offsetBits_posy() {
        return 56;
    }

    /**
     * Return the value (as a int) of the field 'posy'
     */
    public int get_posy() {
        return (int)getUIntBEElement(offsetBits_posy(), 16);
    }

    /**
     * Set the value of the field 'posy'
     */
    public void set_posy(int value) {
        setUIntBEElement(offsetBits_posy(), 16, value);
    }

    /**
     * Return the size, in bytes, of the field 'posy'
     */
    public static int size_posy() {
        return (16 / 8);
    }

    /**
     * Return the size, in bits, of the field 'posy'
     */
    public static int sizeBits_posy() {
        return 16;
    }

    /////////////////////////////////////////////////////////
    // Accessor methods for field: RSSI
    //   Field type: byte, signed
    //   Offset (bits): 72
    //   Size (bits): 8
    /////////////////////////////////////////////////////////

    /**
     * Return whether the field 'RSSI' is signed (true).
     */
    public static boolean isSigned_RSSI() {
        return true;
    }

    /**
     * Return whether the field 'RSSI' is an array (false).
     */
    public static boolean isArray_RSSI() {
        return false;
    }

    /**
     * Return the offset (in bytes) of the field 'RSSI'
     */
    public static int offset_RSSI() {
        return (72 / 8);
    }

    /**
     * Return the offset (in bits) of the field 'RSSI'
     */
    public static int offsetBits_RSSI() {
        return 72;
    }

    /**
     * Return the value (as a byte) of the field 'RSSI'
     */
    public byte get_RSSI() {
        return (byte)getSIntBEElement(offsetBits_RSSI(), 8);
    }

    /**
     * Set the value of the field 'RSSI'
     */
    public void set_RSSI(byte value) {
        setSIntBEElement(offsetBits_RSSI(), 8, value);
    }

    /**
     * Return the size, in bytes, of the field 'RSSI'
     */
    public static int size_RSSI() {
        return (8 / 8);
    }

    /**
     * Return the size, in bits, of the field 'RSSI'
     */
    public static int sizeBits_RSSI() {
        return 8;
    }

}
