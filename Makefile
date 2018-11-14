# GNU Make makefile for building with Mono

MONO    = mono
MSBUILD = msbuild

#CONFIG = Debug
CONFIG = Release

PK3DS_EXE = pk3DS/bin/$(CONFIG)/pk3DS.exe

# Note: The Mono compiler seems to dump some detritus by default in
# /tmp, so we set TMPDIR to a more suitable location

$(PK3DS_EXE): pk3DS.sln
	mkdir -p tmp
	TMPDIR=$(shell pwd)/tmp $(MSBUILD) /p:Configuration=$(CONFIG) $<

install: $(PK3DS_EXE)
	rm -rf pk3ds-install
	mkdir pk3ds-install
	cp -np */bin/$(CONFIG)/*.dll pk3ds-install
	cp -np */bin/$(CONFIG)/*.exe pk3ds-install

run: $(PK3DS_EXE)
	$(MONO) $<

run-install: pk3ds-install/SPICA.exe
	$(MONO) $<

clean:
	rm -rf pk3DS*/bin pk3DS*/obj
	rm -rf pk3ds-install
	rm -rf tmp

.PHONY: clean install run run-install

# EOF
