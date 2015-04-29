using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ip_calculator
{
    class IpCalculator
    {
        public static string GetClass(ref int ipClass) {
            string result = null;

            switch (ipClass) {
                case 1:
                    result = "IP Class: Private block, Class 'A'";
                    ipClass = 1;
                    break;
                case 2:
                    result = "IP Class: Private block, Class 'B'";
                    ipClass = 2;
                    break;
                case 3:
                    result = "IP Class: Private block, Class 'C'";
                    ipClass = 3;
                    break;
                case 4:
                    result = "IP Class: Reserved block, System Loopback Address";
                    ipClass = 1;
                    break;
                case 5:
                    result = "IP Class: A";
                    ipClass = 1;
                    break;
                case 6:
                    result = "IP Class: B";
                    ipClass = 2;
                    break;
                case 7:
                    result = "IP Class: C";
                    ipClass = 3;
                    break;
                case 8:
                    result = "IP Class: D";
                    ipClass = 4;
                    break;
                case 9:
                    result = "IP Class: E";
                    ipClass = 5;
                    break;
                default:
                    result = "Not in Range";
                    break;
            }

            return result;
        }

        public static int CalcClass(int[] octetsIP) {
            if (octetsIP[0] == 10) {
                return 1;
            } else if (octetsIP[0] == 172 && octetsIP[1] >= 16 && octetsIP[1] <= 31) {
                return 2;
            } else if (octetsIP[0] == 192 && octetsIP[1] == 168) {
                return 3;
            } else if (octetsIP[0] == 127) {
                return 4;
            } else if (octetsIP[0] >= 0 && octetsIP[0] < 127) {
                return 5;
            } else if (octetsIP[0] > 127 && octetsIP[0] < 192) {
                return 6;
            } else if (octetsIP[0] > 191 && octetsIP[0] < 224) {
                return 7;
            } else if (octetsIP[0] > 223 && octetsIP[0] < 240) {
                return 8;
            } else if (octetsIP[0] > 239 && octetsIP[0] <= 255) {
                return 9;
            } else {
                return 0;
            }
        }

        public static Int64 GetHostsPerSubnet(int[] octetsIP) {
            int hostBits = 0;
            for (int i = 0; i < octetsIP.Length; i++) {
                if (octetsIP[i] == 255) {
                    hostBits += 0;
                } else if (octetsIP[i] == 254) {
                    hostBits += 1;
                } else if (octetsIP[i] == 252) {
                    hostBits += 2;
                } else if (octetsIP[i] == 248) {
                    hostBits += 3;
                } else if (octetsIP[i] == 240) {
                    hostBits += 4;
                } else if (octetsIP[i] == 224) {
                    hostBits += 5;
                } else if (octetsIP[i] == 192) {
                    hostBits += 6;
                } else if (octetsIP[i] == 128) {
                    hostBits += 7;
                } else if (octetsIP[i] == 0) {
                    hostBits += 8;
                } else {
                    hostBits = 0;
                    break;
                }
            }
            Int64 hostsPerSubnet = Convert.ToInt64(Math.Pow(2.0, hostBits)) - 2;
            return hostsPerSubnet;
        }

        public static int GetSubnets(int[] decimalMask, int ipClass) {
            int netBits = 0;

            int[] subClassMask = new int[4];
            switch (ipClass) {
                case 1:
                    subClassMask = new int[] { 255, 0, 0, 0 };
                    break;
                case 2:
                    subClassMask = new int[] { 255, 255, 0, 0 };
                    break;
                case 3:
                    subClassMask = new int[] { 255, 255, 255, 0 };
                    break;
                case 4:
                case 5:
                    subClassMask = new int[] {
                        decimalMask[0],
                        decimalMask[1],
                        decimalMask[2],
                        decimalMask[3]
                    };
                    break;
            }

            for (int i = 0; i < decimalMask.Length; i++) {
                if (decimalMask[i] != subClassMask[i]) {
                    if (decimalMask[i] == 255) {
                        netBits += 8;
                    } else if (decimalMask[i] == 254) {
                        netBits += 7;
                    } else if (decimalMask[i] == 252) {
                        netBits += 6;
                    } else if (decimalMask[i] == 248) {
                        netBits += 5;
                    } else if (decimalMask[i] == 240) {
                        netBits += 4;
                    } else if (decimalMask[i] == 224) {
                        netBits += 3;
                    } else if (decimalMask[i] == 192) {
                        netBits += 2;
                    } else if (decimalMask[i] == 128) {
                        netBits += 1;
                    } else if (decimalMask[i] == 0) {
                        netBits += 0;
                    } else {
                        netBits += 0;
                    }
                }
            }
            int subnets = Convert.ToInt32(Math.Pow(2.0, netBits));
            return subnets;
        }

        public static int[] GetNetID(int[] octetsIPBits, int[] octetsMaskBits) {
            int[] netID = new int[octetsIPBits.Length];

            for (int j = 0; j < octetsIPBits.Length; j++) {
                netID[j] = octetsIPBits[j] & octetsMaskBits[j];
            }

            return netID;
        }

        public static int[] GetNetIDRange(int[] decimalNetID, int netInc, int[] decimalMask) {
            int[] netIDEnd = new int[decimalNetID.Length];

            for (int i = 0; i < decimalNetID.Length; i++) {
                if (decimalMask[i] == 255) {
                    netIDEnd[i] = decimalNetID[i];
                } else if (decimalMask[i] < 255 && decimalMask[i] > 0) {
                    netIDEnd[i] = decimalNetID[i] + netInc - 1;
                } else {
                    netIDEnd[i] = 255;
                }
            }

            return netIDEnd;
        }

        public static int GetIncrement(int[] decimalMask, int[] decimalNetID) {
            int increment = 0;

            for (int i = 0; i < decimalMask.Length; i++) {
                if (decimalMask[i] == 255) {
                    increment = 1;
                } else if (decimalMask[i] == 254) {
                    increment = 2;
                    break;
                } else if (decimalMask[i] == 252) {
                    increment = 4;
                    break;
                } else if (decimalMask[i] == 248) {
                    increment = 8;
                    break;
                } else if (decimalMask[i] == 240) {
                    increment = 16;
                    break;
                } else if (decimalMask[i] == 224) {
                    increment = 32;
                    break;
                } else if (decimalMask[i] == 192) {
                    increment = 64;
                    break;
                } else if (decimalMask[i] == 128) {
                    increment = 128;
                    break;
                }
            }

            return increment;
        }
    }
}
