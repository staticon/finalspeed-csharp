﻿using FinalSpeed.socket;
using FinalSpeed.utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FinalSpeed.rudp.message
{
    public class AckListMessage : Message
    {
        List<int> ackList;   //ArrayList<Integer> ackList
        byte[] dpData = null;
        int lastRead;

        int r1, r2, r3, s1, s2, s3;


        public AckListMessage(long connId, List<int> ackList, int lastRead, //ArrayList ackList
                Dictionary<int, SendRecord> sendRecordTable, int timeId,
                int connectId, int clientId)
        {
            this.clientId = clientId;
            this.connectId = connectId;
            this.ackList = ackList;
            this.lastRead = lastRead;
            int len1 = 4 + 4 + 10 + 4 * ackList.Count;
            dpData = new byte[len1 + 24 + 9];
            sType = MessageType.sType_AckListMessage;
            ByteShortConvert.toByteArray(ver, dpData, 0);  //add: ver
            ByteShortConvert.toByteArray(sType, dpData, 2);  //add: service type
            ByteIntConvert.toByteArray(connectId, dpData, 4); //add: sequence
            ByteIntConvert.toByteArray(clientId, dpData, 8); //add: sequence

            ByteIntConvert.toByteArray(lastRead, dpData, 4 + 8);
            //dpData[8]=(byte) ackList.size();
            ByteShortConvert.toByteArray((short)ackList.Count, dpData, 8 + 8);  //add: service type
            for (int i = 0; i < ackList.Count; i++)
            {
                int sequence = (int)ackList[i];
                ByteIntConvert.toByteArray(sequence, dpData, 10 + 4 * i + 8);  //add: sequence
                ////#MLog.println("发送确认 "+sequence);
            }

            int u1 = timeId - 2;
            ByteIntConvert.toByteArray(u1, dpData, len1 + 8);
            SendRecord r1 = sendRecordTable[u1];
            int s1 = 0;
            if (r1 != null)
            {
                s1 = r1.getSendSize();
            }
            ByteIntConvert.toByteArray(s1, dpData, len1 + 4 + 8);

            int u2 = timeId - 1;
            ByteIntConvert.toByteArray(u2, dpData, len1 + 8 + 8);
            SendRecord r2 = sendRecordTable[u2];
            int s2 = 0;
            if (r2 != null)
            {
                s2 = r2.getSendSize();
            }
            ByteIntConvert.toByteArray(s2, dpData, len1 + 12 + 8);

            int u3 = timeId;
            ByteIntConvert.toByteArray(u3, dpData, len1 + 16 + 8);
            SendRecord r3 = sendRecordTable[u3];
            int s3 = 0;
            if (r3 != null)
            {
                s3 = r3.getSendSize();
            }
            ByteIntConvert.toByteArray(s3, dpData, len1 + 20 + 8);

            dp = new DatagramPacket(dpData, dpData.Length);

        }

        public List<int> getAckList()
        {
            return ackList;
        }

        public AckListMessage(DatagramPacket dp)
        {
            this.dp = dp;
            dpData = dp.getData();
            ver = ByteShortConvert.toShort(dpData, 0);
            sType = ByteShortConvert.toShort(dpData, 2);
            connectId = ByteIntConvert.toInt(dpData, 4);
            clientId = ByteIntConvert.toInt(dpData, 8);


            lastRead = ByteIntConvert.toInt(dpData, 4 + 8);
            int sum = ByteShortConvert.toShort(dpData, 8 + 8);
            ackList = new List<int>();
            int t = 0;
            for (int i = 0; i < sum; i++)
            {
                t = 10 + 4 * i;
                int sequence = ByteIntConvert.toInt(dpData, t + 8);
                ackList.Add(sequence);
            }
            ////#MLog.println("LLLLLLLLLLLLLL "+dp.getLength()+" "+ackList.size());
            t = 10 + 4 * sum - 4;
            r1 = ByteIntConvert.toInt(dpData, t + 4 + 8);
            s1 = ByteIntConvert.toInt(dpData, t + 8 + 8);

            r2 = ByteIntConvert.toInt(dpData, t + 12 + 8);
            s2 = ByteIntConvert.toInt(dpData, t + 16 + 8);

            r3 = ByteIntConvert.toInt(dpData, t + 20 + 8);
            s3 = ByteIntConvert.toInt(dpData, t + 24 + 8);

            ////#MLog.println("aaaaaaaaa"+r3+"kkkkkkk "+s3);
        }

        public int getLastRead()
        {
            return lastRead;
        }

        public int getR1()
        {
            return r1;
        }

        public int getR3()
        {
            return r3;
        }

        public int getS1()
        {
            return s1;
        }

        public int getS2()
        {
            return s2;
        }

        public int getS3()
        {
            return s3;
        }

        public int getR2()
        {
            return r2;
        }
    }
}
