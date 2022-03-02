#!/bin/bash

apt-get update -y
apt-get install -y tzdata 
apt-get install -y build-essential cmake

cd /src/wasi-lab/qjs-wasi/
./build.sh
