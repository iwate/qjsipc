#!/bin/bash

apt-get update
apt-get install -y build-essential cmake

cd /src/wasi-lab/qjs-wasi/
./build.sh
