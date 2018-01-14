#apt-get: Install key and add mono source
sudo apt-key adv --keyserver hkp://keyserver.ubuntu.com:80 --recv-keys 3FA7E0328081BFF6A14DA29AA6A19B38D3D831EF
echo "deb http://download.mono-project.com/repo/ubuntu trusty main" | sudo tee /etc/apt/sources.list.d/mono-official.list

#Update packages to get newest mono able to do nuget deploy
sudo apt-get clean
sudo apt-get update