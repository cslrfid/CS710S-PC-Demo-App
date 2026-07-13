using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//HID_KEYBOARD_A              4    // 0x04 - Keyboard a and A
//HID_KEYBOARD_B              5    // 0x05 - Keyboard b and B
//HID_KEYBOARD_C              6    // 0x06 - Keyboard c and C
//HID_KEYBOARD_D              7    // 0x07 - Keyboard d and D
//HID_KEYBOARD_E              8    // 0x08 - Keyboard e and E
//HID_KEYBOARD_F              9    // 0x09 - Keyboard f and F
//HID_KEYBOARD_G              10   // 0x0A - Keyboard g and G
//HID_KEYBOARD_H              11   // 0x0B - Keyboard h and H
//HID_KEYBOARD_I              12   // 0x0C - Keyboard i and I
//HID_KEYBOARD_J              13   // 0x0D - Keyboard j and J
//HID_KEYBOARD_K              14   // 0x0E - Keyboard k and K
//HID_KEYBOARD_L              15   // 0x0F - Keyboard l and L
//HID_KEYBOARD_M              16   // 0x10 - Keyboard m and M
//HID_KEYBOARD_N              17   // 0x11 - Keyboard n and N
//HID_KEYBOARD_O              18   // 0x12 - Keyboard o and O
//HID_KEYBOARD_P              19   // 0x13 - Keyboard p and p
//HID_KEYBOARD_Q              20   // 0x14 - Keyboard q and Q
//HID_KEYBOARD_R              21   // 0x15 - Keyboard r and R
//HID_KEYBOARD_S              22   // 0x16 - Keyboard s and S
//HID_KEYBOARD_T              23   // 0x17 - Keyboard t and T
//HID_KEYBOARD_U              24   // 0x18 - Keyboard u and U
//HID_KEYBOARD_V              25   // 0x19 - Keyboard v and V
//HID_KEYBOARD_W              26   // 0x1A - Keyboard w and W
//HID_KEYBOARD_X              27   // 0x1B - Keyboard x and X
//HID_KEYBOARD_Y              28   // 0x1C - Keyboard y and Y
//HID_KEYBOARD_Z              29   // 0x1D - Keyboard z and Z
//HID_KEYBOARD_1              30   // 0x1E - Keyboard 1 and !
//HID_KEYBOARD_2              31   // 0x1F - Keyboard 2 and @
//HID_KEYBOARD_3              32   // 0x20 - Keyboard 3 and #
//HID_KEYBOARD_4              33   // 0x21 - Keyboard 4 and %
//HID_KEYBOARD_5              34   // 0x22 - Keyboard 5 and %
//HID_KEYBOARD_6              35   // 0x23 - Keyboard 6 and ^
//HID_KEYBOARD_7              36   // 0x24 - Keyboard 7 and &
//HID_KEYBOARD_8              37   // 0x25 - Keyboard 8 and *
//HID_KEYBOARD_9              38   // 0x26 - Keyboard 9 and (
//HID_KEYBOARD_0              39   // 0x27 - Keyboard 0 and)
//HID_KEYBOARD_RETURN         40   // 0x28 - Keyboard Return (ENTER)
//HID_KEYBOARD_ESCAPE         41   // 0x29 - Keyboard ESCAPE
//HID_KEYBOARD_DELETE         42   // 0x2A - Keyboard DELETE (Backspace)
//HID_KEYBOARD_TAB            43   // 0x2B - Keyboard Tab
//HID_KEYBOARD_SPACEBAR       44   // 0x2C - Keyboard Spacebar
//HID_KEYBOARD_MINUS          45   // 0x2D - Keyboard - and (underscore)
//HID_KEYBOARD_EQUAL          46   // 0x2E - Keyboard = and +
//HID_KEYBOARD_LEFT_BRKT      47   // 0x2F - Keyboard [ and {
//HID_KEYBOARD_RIGHT_BRKT     48   // 0x30 - Keyboard ] and }
//HID_KEYBOARD_BACK_SLASH     49   // 0x31 - Keyboard \ and |
//HID_KEYBOARD_SEMI_COLON     51   // 0x33 - Keyboard ; and :
//HID_KEYBOARD_SGL_QUOTE      52   // 0x34 - Keyboard ' and "
//HID_KEYBOARD_GRV_ACCENT     53   // 0x35 - Keyboard Grave Accent and Tilde
//HID_KEYBOARD_COMMA          54   // 0x36 - Keyboard , and <
//HID_KEYBOARD_DOT            55   // 0x37 - Keyboard . and >
//HID_KEYBOARD_FWD_SLASH      56   // 0x38 - Keyboard / and ?

namespace CS710_PC_Client
{
    static class Keyboard
    {
        public static int GetKeyCode(string key)
        {
            int keycode = 0;

            switch (key)
            {
                case "a":
                case "A":
                    keycode = 4;
                    break;
                case "b":
                case "B":
                    keycode = 5;
                    break;
                case "c":
                case "C":
                    keycode = 6;
                    break;
                case "d":
                case "D":
                    keycode = 7;
                    break;
                case "e":
                case "E":
                    keycode = 8;
                    break;
                case "f":
                case "F":
                    keycode = 9;
                    break;
                case "g":
                case "G":
                    keycode = 10;
                    break;
                case "h":
                case "H":
                    keycode = 11;
                    break;
                case "i":
                case "I":
                    keycode = 12;
                    break;
                case "j":
                case "J":
                    keycode = 13;
                    break;
                case "k":
                case "K":
                    keycode = 14;
                    break;
                case "l":
                case "L":
                    keycode = 15;
                    break;
                case "m":
                case "M":
                    keycode = 16;
                    break;
                case "n":
                case "N":
                    keycode = 17;
                    break;
                case "o":
                case "O":
                    keycode = 18;
                    break;
                case "p":
                case "P":
                    keycode = 19;
                    break;
                case "q":
                case "Q":
                    keycode = 20;
                    break;
                case "r":
                case "R":
                    keycode = 21;
                    break;
                case "s":
                case "S":
                    keycode = 22;
                    break;
                case "t":
                case "T":
                    keycode = 23;
                    break;
                case "u":
                case "U":
                    keycode = 24;
                    break;
                case "v":
                case "V":
                    keycode = 25;
                    break;
                case "w":
                case "W":
                    keycode = 26;
                    break;
                case "x":
                case "X":
                    keycode = 27;
                    break;
                case "y":
                case "Y":
                    keycode = 28;
                    break;
                case "z":
                case "Z":
                    keycode = 29;
                    break;
                case "1":
                case "!":
                    keycode = 30;
                    break;
                case "2":
                case "@":
                    keycode = 31;
                    break;
                case "3":
                case "#":
                    keycode = 32;
                    break;
                case "4":
                case "$":
                    keycode = 33;
                    break;
                case "5":
                case "%":
                    keycode = 34;
                    break;
                case "6":
                case "^":
                    keycode = 35;
                    break;
                case "7":
                case "&":
                    keycode = 36;
                    break;
                case "8":
                case "*":
                    keycode = 37;
                    break;
                case "9":
                case "(":
                    keycode = 38;
                    break;
                case "0":
                case ")":
                    keycode = 39;
                    break;
                case "Enter":
                    keycode = 40;
                    break;
                case "Tab":
                    keycode = 43;
                    break;
                case "Space":
                    keycode = 44;
                    break;
                case "-":
                case "_":
                    keycode = 45;
                    break;
                case "=":
                case "+":
                    keycode = 46;
                    break;
                case "[":
                case "{":
                    keycode = 47;
                    break;
                case "]":
                case "}":
                    keycode = 48;
                    break;
                case "\\":
                case "|":
                    keycode = 49;
                    break;
                case ";":
                case ":":
                    keycode = 51;
                    break;
                case "'":
                case "\"":
                    keycode = 52;
                    break;
                case ",":
                case "<":
                    keycode = 54;
                    break;
                case ".":
                case ">":
                    keycode = 55;
                    break;
                case "/":
                case "?":
                    keycode = 56;
                    break;
                default:
                    keycode = 0;
                    break;
            }

            return keycode;
        }

        public static string GetKeyString(int code, bool shift)
        {
            string key;

            switch (code)
            {
                case 4:
                    key = (shift) ? "A" : "a";
                    break;
                case 5:
                    key = (shift) ? "B" : "b";
                    break;
                case 6:
                    key = (shift) ? "C" : "c";
                    break;
                case 7:
                    key = (shift) ? "D" : "d";
                    break;
                case 8:
                    key = (shift) ? "E" : "e";
                    break;
                case 9:
                    key = (shift) ? "F" : "f";
                    break;
                case 10:
                    key = (shift) ? "G" : "g";
                    break;
                case 11:
                    key = (shift) ? "H" : "h";
                    break;
                case 12:
                    key = (shift) ? "I" : "i";
                    break;
                case 13:
                    key = (shift) ? "J" : "j";
                    break;
                case 14:
                    key = (shift) ? "K" : "k";
                    break;
                case 15:
                    key = (shift) ? "L" : "l";
                    break;
                case 16:
                    key = (shift) ? "M" : "m";
                    break;
                case 17:
                    key = (shift) ? "N" : "n";
                    break;
                case 18:
                    key = (shift) ? "O" : "o";
                    break;
                case 19:
                    key = (shift) ? "P" : "p";
                    break;
                case 20:
                    key = (shift) ? "Q" : "q";
                    break;
                case 21:
                    key = (shift) ? "R" : "r";
                    break;
                case 22:
                    key = (shift) ? "S" : "s";
                    break;
                case 23:
                    key = (shift) ? "T" : "t";
                    break;
                case 24:
                    key = (shift) ? "U" : "u";
                    break;
                case 25:
                    key = (shift) ? "V" : "v";
                    break;
                case 26:
                    key = (shift) ? "W" : "w";
                    break;
                case 27:
                    key = (shift) ? "X" : "x";
                    break;
                case 28:
                    key = (shift) ? "Y" : "y";
                    break;
                case 29:
                    key = (shift) ? "Z" : "z";
                    break;
                case 30:
                    key = (shift) ? "!" : "1";
                    break;
                case 31:
                    key = (shift) ? "@" : "2";
                    break;
                case 32:
                    key = (shift) ? "#" : "3";
                    break;
                case 33:
                    key = (shift) ? "$" : "4";
                    break;
                case 34:
                    key = (shift) ? "%" : "5";
                    break;
                case 35:
                    key = (shift) ? "^" : "6";
                    break;
                case 36:
                    key = (shift) ? "&" : "7";
                    break;
                case 37:
                    key = (shift) ? "*" : "8";
                    break;
                case 38:
                    key = (shift) ? "(" : "9";
                    break;
                case 39:
                    key = (shift) ? ")" : "0";
                    break;
                case 40:
                    key = "Enter";
                    break;
                case 43:
                    key = "Tab";
                    break;
                case 44:
                    key = "Space";
                    break;
                case 45:
                    key = (shift) ? "_" : "-";
                    break;
                case 46:
                    key = (shift) ? "+" : "=";
                    break;
                case 47:
                    key = (shift) ? "{" : "[";
                    break;
                case 48:
                    key = (shift) ? "}" : "]";
                    break;
                case 49:
                    key = (shift) ? "|" : "\\";
                    break;
                case 51:
                    key = (shift) ? ":" : ";";
                    break;
                case 52:
                    key = (shift) ? "\"" : "'";
                    break;
                case 54:
                    key = (shift) ? "<" : ",";
                    break;
                case 55:
                    key = (shift) ? ">" : ".";
                    break;
                case 56:
                    key = (shift) ? "?" : "/";
                    break;
                default:
                    key = "(empty)";
                    break;
            }

            return key;
        }
    }
}
