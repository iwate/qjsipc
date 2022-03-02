#!/bin/bash

apt-get update -y
apt-get install -y tzdata 
apt-get install -y build-essential cmake curl python3 python3-pip libxkbcommon-x11-0 libtinfo5

cd /src/wasi-lab/qjs-wasi/
./build.sh
