FROM phusion/baseimage
MAINTAINER Ryan Roberts <ryansroberts@gmail.com>

RUN apt-get update && apt-key adv --keyserver pgp.mit.edu --recv-keys 3FA7E0328081BFF6A14DA29AA6A19B38D3D831EF

RUN echo "deb http://download.mono-project.com/repo/debian wheezy/snapshots/3.12.0 main" > /etc/apt/sources.list.d/mono-xamarin.list &&\
	  apt-get update &&\
    apt-get install -yy mono-devel ca-certificates-mono fsharp mono-vbnc nuget &&\
	apt-get clean &&\
    rm -rf /var/lib/apt/lists/* /tmp/* /var/tmp/*
	
ADD . /etc/hdas2-ui/

RUN /etc/hdas2-ui/build.sh

CMD mono /etc/hdas2-ui/bin/HDAS-ui/HDAS-ui.exe

EXPOSE 8082